using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TransPoster.Mvc.DataTables.Attributes;
using TransPoster.Mvc.DataTables.Helpers;
using TransPoster.Mvc.DataTables.Model;

namespace TransPoster.Mvc.DataTables;

public abstract class AggreagateDataTableProccesserBase<TEntity, TViewModel>
    where TEntity : class
{
    #region fields

    private static readonly Lazy<PropertyInfo[]> viewModelProperties = new(() => typeof(TViewModel)
        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        .ToArray());

    #endregion

    #region invoke

    public async Task<AjaxData> ProccessAsync(IQueryable<TEntity> source, AjaxDataRequest dataRequest)
    {
        var all = GroupBy(source);

        var totalCount = await all.CountAsync();

        if (totalCount == 0)
        {
            return AjaxData.Empty(dataRequest);
        }

        var filtered = ApplyFilters(source, dataRequest);

        if (!string.IsNullOrEmpty(dataRequest.Search?.Value))
        {
            filtered = ApplyGlobalSearch(filtered, dataRequest);
        }

        var grouped = GroupBy(filtered);

        grouped = ApplyHaving(grouped, dataRequest);

        var filteredCount = await grouped.CountAsync();

        var ordered = Order(grouped, dataRequest);

        var paging = ordered.Paging(dataRequest);

        var data = await paging.ToListAsync();

        return new AjaxData()
        {
            Draw = dataRequest.Draw,
            RecordsTotal = totalCount,
            RecordsFiltered = filteredCount,
            Data = data
        };
    }

    protected abstract IQueryable<TViewModel> GroupBy(IQueryable<TEntity> source);

    #endregion

    #region filter

    protected virtual IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> source, AjaxDataRequest dataRequest)
    {
        foreach (var column in dataRequest.Columns.Where(c => !string.IsNullOrEmpty(c.Search?.Value)))
        {
            var nameOrData = column.Name ?? column.Data;
            Expression<Func<TEntity, bool>> filterExpression = GetFilterExpression(nameOrData, column.Search.Value);

            if (filterExpression != null)
            {
                source = source.Where(filterExpression);
            }
        }

        return source;
    }

    protected virtual IQueryable<TEntity> ApplyGlobalSearch(IQueryable<TEntity> source, AjaxDataRequest request)
    {
        var properties = from p in viewModelProperties.Value
                         where p.GetCustomAttribute<SourceFieldAttribute>()?.GlobalSearch == true
                         select p.Name;

        var expressions = properties.Select(p => GetFilterExpression(p, request.Search.Value));
        var orExpression = ExpressionHelper.CreateOrExpression<TEntity>(expressions.ToArray());
        source = source.Where(orExpression);

        return source;
    }

    protected virtual Expression<Func<TEntity, bool>> GetFilterExpression(string propertyName, string key)
    {
        var property = GetProperty(propertyName);
        return QueryableFilterHelper.GetFilterExpression<TEntity>(property, key);
    }

    protected virtual IQueryable<TViewModel> ApplyHaving(IQueryable<TViewModel> source, AjaxDataRequest dataRequest)
    {
        foreach (var column in dataRequest.Columns.Where(c => !string.IsNullOrEmpty(c.Search?.Value)))
        {
            var nameOrData = column.Name ?? column.Data;

            var filterExpression = GetHavingExpression(nameOrData, column.Search.Value);

            if (filterExpression != null)
            {
                source = source.Where(filterExpression);
            }
        }

        return source;
    }

    protected virtual Expression<Func<TViewModel, bool>> GetHavingExpression(string propertyName, string key)
    {
        var property = GetProperty(propertyName);

        var aggregateField = property.GetCustomAttribute<AggreagateFieldAttribute>();
        var columnSettings = property.GetCustomAttribute<ColumnSettingsAttribute>();

        if (aggregateField is { } && columnSettings is { })
        {
            return ProccesserHelpers.CreateExpression<TViewModel>(propertyName, columnSettings.FilterType, key);
        }

        return null;
    }

    #endregion

    #region order

    protected virtual IOrderedQueryable<TViewModel> Order(IQueryable<TViewModel> source, AjaxDataRequest dataRequest)
    {
        IQueryable<TViewModel> ordered = source;
        bool hasOrder = false;

        foreach (var order in dataRequest.Order)
        {
            var column = dataRequest.Columns[order.Column];
            var columnName = column.Name ?? column.Data;

            if (string.IsNullOrEmpty(columnName))
                continue;

            LambdaExpression expression = GetOrderByExpression(columnName);

            if (expression == null)
                continue;

            ordered = ordered.AddOrderLevel(order.Dir, expression, hasOrder);
            hasOrder = true;
        }

        var result = OrderByKey(ordered, hasOrder);
        return result;
    }

    protected virtual LambdaExpression GetOrderByExpression(string propertyName)
    {
        return QueryHelper.GetMemberExpression<TViewModel>(propertyName);
    }

    #endregion

    protected PropertyInfo GetProperty(string name) => viewModelProperties.Value.FirstOrDefault(p => p.Name == name);

    #region order helpers

    protected virtual IOrderedQueryable<TViewModel> OrderByKey(IQueryable<TViewModel> source, bool hasOrder)
    {
        string propertyName;
        OrderDirection orderDirection;

        var keyProperty = (from p in viewModelProperties.Value
                           let att = p.GetCustomAttribute<KeyFieldAttribute>()
                           where att != null
                           select (
                               propertyName = p.Name,
                               att.OrderDirection
                           )).FirstOrDefault();

        if (keyProperty is { })
        {
            (propertyName, orderDirection) = keyProperty;
        }
        else
        {
            propertyName = viewModelProperties.Value.First().Name;
            orderDirection = OrderDirection.Asc;
        }


        var member = QueryHelper.GetMemberExpression<TViewModel>(propertyName);
        return source.AddOrderLevel(orderDirection, member, hasOrder);
    }


    #endregion
}

public sealed class SimpleAggreagateDataTableProccesser<TEntity, TViewModel> : AggreagateDataTableProccesserBase<TEntity, TViewModel>
    where TEntity : class
{
    private readonly Func<IQueryable<TEntity>, IQueryable<TViewModel>> group;

    public SimpleAggreagateDataTableProccesser(Func<IQueryable<TEntity>, IQueryable<TViewModel>> group)
    {
        this.group = group;
    }

    protected override IQueryable<TViewModel> GroupBy(IQueryable<TEntity> source) => group(source);
}
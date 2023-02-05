using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TransPoster.Mvc.DataTables.Attributes;
using TransPoster.Mvc.DataTables.Helpers;
using TransPoster.Mvc.DataTables.Model;

namespace TransPoster.Mvc.DataTables;

public abstract class DataTableProccesserBase<TEntity, TViewModel> : IDataProccesser<TEntity, TViewModel>
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
        var totalCount = await source.CountAsync();

        if (totalCount == 0)
        {
            return AjaxData.Empty(dataRequest);
        }

        var filtered = ApplyFilters(source, dataRequest);

        if (!string.IsNullOrEmpty(dataRequest.Search?.Value))
        {
            filtered = ApplyGlobalSearch(filtered, dataRequest);
        }

        var filteredCount = await filtered.CountAsync();

        var ordered = Order(filtered, dataRequest);

        var paging = ordered.Paging(dataRequest);

        var data = await DoQueryAsync(paging);

        return new AjaxData()
        {
            Draw = dataRequest.Draw,
            RecordsTotal = totalCount,
            RecordsFiltered = filteredCount,
            Data = data
        };
    }

    protected abstract Task<IEnumerable<TViewModel>> DoQueryAsync(IQueryable<TEntity> query);

    #endregion

    #region filter

    protected virtual IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> source, AjaxDataRequest dataRequest)
    {
        foreach (var column in dataRequest.Columns.Where(c => !string.IsNullOrEmpty(c.Search?.Value)))
        {
            var nameOrData = column.Name ?? column.Data;
            source = ApplyFilter(source, columnName: nameOrData, searchValue: column.Search.Value);
        }

        return source;
    }

    protected virtual IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> source, string columnName, string searchValue)
    {
        Expression<Func<TEntity, bool>>? filterExpression = GetFilterExpression(columnName, searchValue);

        if (filterExpression is not null)
        {
            source = source.Where(filterExpression);
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

    protected virtual Expression<Func<TEntity, bool>>? GetFilterExpression(string propertyName, string key)
    {
        var property = GetProperty(propertyName);

        return property is null ? null : QueryableFilterHelper.GetFilterExpression<TEntity>(property, key);
    }

    #endregion

    #region order

    protected virtual IOrderedQueryable<TEntity> Order(IQueryable<TEntity> source, AjaxDataRequest dataRequest)
    {
        IQueryable<TEntity> ordered = source;
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
        var property = GetProperty(propertyName);
        if (property == null) return null;

        var path = AttributeHelper.GetOrderPath(property);

        if (path != null)
        {
            return QueryHelper.GetMemberExpression<TEntity>(path);
        }

        return null;
    }

    #endregion

    protected PropertyInfo? GetProperty(string name) => viewModelProperties.Value.FirstOrDefault(p => p.Name == name);

    #region order helpers

    protected virtual IOrderedQueryable<TEntity> OrderByKey(IQueryable<TEntity> source, bool hasOrder)
    {
        var (name, direction) = AttributeHelper.GetKeyOrder(typeof(TEntity), typeof(TViewModel));

        var member = QueryHelper.GetMemberExpression<TEntity>(name);
        return source.AddOrderLevel(direction, member, hasOrder);
    }

    #endregion

    protected static LambdaExpression CreateLambdaExpression<TVal>(Expression<Func<TEntity, TVal>> expression) => expression;

    #region IDataProccesser explicit implementation

    Task<IEnumerable<TViewModel>> IDataProccesser<TEntity, TViewModel>.DoQueryAsync(IQueryable<TEntity> query) => DoQueryAsync(query);

    IQueryable<TEntity> IDataProccesser<TEntity, TViewModel>.ApplyFilters(IQueryable<TEntity> source, AjaxDataRequest dataRequest) => ApplyFilters(source, dataRequest);

    #endregion
}
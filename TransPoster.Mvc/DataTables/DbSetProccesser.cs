using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TransPoster.Data.Models;
using TransPoster.Mvc.DataTables.Helpers;
using TransPoster.Mvc.DataTables.Model;
using TransPoster.Mvc.Extensions;

namespace TransPoster.Mvc.DataTables;


public sealed class DbSetProccesser
{
    #region ctor, fields

    private readonly DbContext db;

    public DbSetProccesser(DbContext db)
    {
        this.db = db ?? throw new ArgumentNullException(nameof(db));
    }

    #endregion

    public async Task<AjaxData> ProccessAsync(string typeName, AjaxDataRequest dataRequest)
    {
        try
        {
            var declaringType = Type.GetType(typeName, throwOnError: false);
            MethodInfo setMethod = db.GetType().GetMethod("Set")
                .MakeGenericMethod(declaringType);
            object dbSet = setMethod.Invoke(db, null);

            var queryable = dbSet.GetType().GetMethod("AsQueryable").Invoke(dbSet, null) as IQueryable<object>;

            Type dbSetProccesserType = typeof(DbSetProccesser<>).MakeGenericType(declaringType);
            object dbSetProccesser = Activator.CreateInstance(dbSetProccesserType);
            MethodInfo proccessMethod = dbSetProccesserType.GetMethod("ProccessAsync", new Type[] { typeof(IQueryable<object>), typeof(AjaxDataRequest) });
            AjaxData result = await (Task<AjaxData>)proccessMethod.Invoke(dbSetProccesser, new object[] { queryable, dataRequest });
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}

public sealed class DbSetProccesser<TEntity> where TEntity : class
{
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

        var data = await paging.AsNoTracking().ToListAsync();

        return new AjaxData()
        {
            Draw = dataRequest.Draw,
            RecordsTotal = totalCount,
            RecordsFiltered = filteredCount,
            Data = data
        };
    }

    private IQueryable<TEntity> ApplyGlobalSearch(IQueryable<TEntity> filtered, AjaxDataRequest dataRequest)
    {
        throw new NotImplementedException();
    }

    private IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> source, AjaxDataRequest dataRequest)
    {
        throw new NotImplementedException();
    }

    private IQueryable<TEntity> Order(IQueryable<TEntity> source, AjaxDataRequest dataRequest)
    {
        throw new NotImplementedException();
    }
}
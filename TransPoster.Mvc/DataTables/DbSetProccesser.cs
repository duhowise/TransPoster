using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TransPoster.Mvc.DataTables.Helpers;
using TransPoster.Mvc.DataTables.Model;

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
            var dbsetProcessor = typeof(DbSetProccesser<>).MakeGenericType(declaringType);
            var dbset = db.GetType().GetProperty(declaringType.Name).GetValue(db);
            
            var dbProcessorInstance = Activator.CreateInstance(dbsetProcessor);
            var task = (Task)dbProcessorInstance.GetType().GetMethod("ProccessAsync", new Type[] { dbset.GetType(), dataRequest.GetType() })
                .Invoke(dbProcessorInstance, new []{ dbset, dataRequest });
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            return (AjaxData)resultProperty.GetValue(task);
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
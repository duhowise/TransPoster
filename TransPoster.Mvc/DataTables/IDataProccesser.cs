using TransPoster.Mvc.DataTables.Model;

namespace TransPoster.Mvc.DataTables;

public interface IDataProccesser<TEntity, TViewModel>
    where TEntity : class
{
    Task<IEnumerable<TViewModel>> DoQueryAsync(IQueryable<TEntity> query);
    IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> source, AjaxDataRequest dataRequest);
}
using Microsoft.EntityFrameworkCore;

namespace TransPoster.Mvc.DataTables;

public abstract class LocalizeProccesserBase<TEntity, TViewModel> : DataTableProccesserBase<TEntity, TViewModel>
    where TEntity : class
    where TViewModel : class
{
    protected override async Task<IEnumerable<TViewModel>> DoQueryAsync(IQueryable<TEntity> query)
    {
        var source = await query.ToListAsync();
        var result = new List<TViewModel>();

        foreach (var s in source)
        {
            var r = await ConvertAsync(s);
            result.Add(r);
        }

        return result;
    }

    protected abstract Task<TViewModel> ConvertAsync(TEntity model);
}
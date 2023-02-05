using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TransPoster.Mvc.DataTables;

public abstract class RemoteProccesserBase<TEntity, TViewModel> : DataTableProccesserBase<TEntity, TViewModel>
    where TEntity : class
{
    protected override async Task<IEnumerable<TViewModel>> DoQueryAsync(IQueryable<TEntity> query)
    {
        var vmQuery = query.Select(Convert());
        var data = await vmQuery.ToListAsync();
        await ProccessLocalAsync(data);
        return data;
    }

    protected abstract Expression<Func<TEntity, TViewModel>> Convert();

    protected virtual async Task ProccessLocalAsync(IEnumerable<TViewModel> data)
    {
        foreach (var item in data)
        {
            await ProccessItemAsync(item);
        }
    }

    protected virtual Task ProccessItemAsync(TViewModel viewModel) => Task.CompletedTask;
}
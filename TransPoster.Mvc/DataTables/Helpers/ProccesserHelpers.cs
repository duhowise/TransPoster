using System.Linq.Expressions;

namespace TransPoster.Mvc.DataTables.Helpers;

public static class ProccesserHelpers
{
    public static Expression<Func<TEntity, bool>>? CreateExpression<TEntity>(string path, FilterType filterType, string key)
    {
        var selector = QueryHelper.GetMemberExpression<TEntity>(path);

        return filterType switch
        {
            FilterType.Text => QueryHelper.Contains<TEntity>(selector, key),
            FilterType.List => QueryHelper.InList<TEntity>(selector, key),
            FilterType.DateRange => QueryHelper.Between<TEntity>(selector, key),
            FilterType.Date or FilterType.Boolean => QueryHelper.IsEquals<TEntity>(selector, key),
            _ => null,
        };
    }
}
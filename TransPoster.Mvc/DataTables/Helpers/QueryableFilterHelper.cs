using System.Linq.Expressions;
using System.Reflection;
using TransPoster.Mvc.DataTables.Attributes;

namespace TransPoster.Mvc.DataTables.Helpers;

public static class QueryableFilterHelper
{
    public static Expression<Func<TEntity, bool>> GetFilterExpression<TEntity>(PropertyInfo property, string key)
    {
        var path = AttributeHelper.GetFilterPath(property);

        if (path != null)
        {
            var columnSettings = property.GetCustomAttribute<ColumnSettingsAttribute>(true);
            var filterType = columnSettings?.FilterType;

            if (filterType == null)
            {
                var source = property.GetCustomAttribute<SourceFieldAttribute>(true);
                if (source != null)
                {
                    filterType = source.FilterType ?? FilterType.Text;
                }
            }

            if (filterType.HasValue)
            {
                return ProccesserHelpers.CreateExpression<TEntity>(path, filterType.Value, key);
            }
        }

        return null;
    }

}
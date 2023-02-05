using TransPoster.Mvc.DataTables.Model;

namespace TransPoster.Mvc.DataTables.Helpers;

public static class PagingHelper
{
    public static IQueryable<T> Paging<T>(this IQueryable<T> source, AjaxDataRequest ajaxDataRequest)
    {
        return source.Paging(start: ajaxDataRequest.Start, length: ajaxDataRequest.Length);
    }

    private static IQueryable<T> Paging<T>(this IQueryable<T> source, int start, int length)
    {
        IQueryable<T> query = source;

        if (start > 0)
        {
            query = query.Skip(start);
        }

        if (length > -1)
        {
            query = query.Take(length);
        }

        return query;
    }
}

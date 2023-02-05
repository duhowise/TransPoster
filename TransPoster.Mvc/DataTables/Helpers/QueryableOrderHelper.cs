using System.Linq.Expressions;
using System.Reflection;
using TransPoster.Mvc.DataTables.Model;

namespace TransPoster.Mvc.DataTables.Helpers;

public static class QueryableOrderHelper
{
    public static IOrderedQueryable<TSource> AddOrderLevel<TSource>(this IQueryable<TSource> source, OrderDirection orderDirection, LambdaExpression expression, bool isOrdered)
         => isOrdered ?
         ThenBy(orderDirection, source as IOrderedQueryable<TSource>, expression) :
         OrderBy(orderDirection, source, expression);

    private static IOrderedQueryable<TSource> OrderBy<TSource>(OrderDirection orderDirection, IQueryable<TSource> source, LambdaExpression expression)
        => orderDirection == OrderDirection.Asc ? source.OrderBy(expression) : source.OrderByDescending(expression);

    private static IOrderedQueryable<TSource> ThenBy<TSource>(OrderDirection orderDirection, IOrderedQueryable<TSource> source, LambdaExpression expression)
        => orderDirection == OrderDirection.Asc ? source.ThenBy(expression) : source.ThenByDescending(expression);

    private static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, LambdaExpression expression)
        => InvokeOrderMethod(OrderByMethod, source, expression);

    private static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, LambdaExpression expression)
        => InvokeOrderMethod(OrderByDescMethod, source, expression);

    private static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, LambdaExpression expression)
        => InvokeOrderMethod(ThenByMethod, source, expression);

    private static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IOrderedQueryable<TSource> source, LambdaExpression expression)
        => InvokeOrderMethod(ThenByDescMethod, source, expression);

    private static IOrderedQueryable<TSource> InvokeOrderMethod<TSource>(MethodInfo method, IQueryable<TSource> source, LambdaExpression expression)
    {
        MethodInfo genericMethod = method.MakeGenericMethod(new[] { typeof(TSource), expression.ReturnType });
        object ret = genericMethod.Invoke(null, new object[] { source, expression });
        return (IOrderedQueryable<TSource>)ret;
    }

    #region static readonly methods

    private static readonly MethodInfo OrderByMethod =
        typeof(Queryable).GetMethods()
            .Where(method => method.Name == nameof(Queryable.OrderBy))
            .Where(method => method.GetParameters().Length == 2)
            .Single();

    private static readonly MethodInfo OrderByDescMethod =
        typeof(Queryable).GetMethods()
            .Where(method => method.Name == nameof(Queryable.OrderByDescending))
            .Where(method => method.GetParameters().Length == 2)
            .Single();

    private static readonly MethodInfo ThenByMethod =
        typeof(Queryable).GetMethods()
            .Where(method => method.Name == nameof(Queryable.ThenBy))
            .Where(method => method.GetParameters().Length == 2)
            .Single();

    private static readonly MethodInfo ThenByDescMethod =
        typeof(Queryable).GetMethods()
            .Where(method => method.Name == nameof(Queryable.ThenByDescending))
            .Where(method => method.GetParameters().Length == 2)
            .Single();

    #endregion // static readonly methods
}
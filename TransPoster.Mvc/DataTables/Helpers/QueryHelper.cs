using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using TransPoster.Mvc.Extensions;

namespace TransPoster.Mvc.DataTables.Helpers;

public static class QueryHelper
{
    private static readonly Lazy<MethodInfo> EnumerableContains = new(() =>
       typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
           .Where(method => method.Name == nameof(Enumerable.Contains))
           .Where(method => method.GetParameters().Length == 2)
           .Single()
        );

    private static readonly Lazy<MethodInfo> EnumerableCust = new(() => typeof(Enumerable).GetMethod(nameof(Enumerable.Cast), BindingFlags.Static | BindingFlags.Public)!);

    public static Expression<Func<TEntity, bool>> IsEquals<TEntity>(LambdaExpression selector, string searchKey)
    {
        var val = Convert.ChangeType(searchKey, selector.ReturnType.GetNonNullableType());
        return ExpressionHelper.CreateEqualExpression<TEntity>(selector, val);
    }

    public static Expression<Func<TEntity, bool>> IsEquals<TEntity, TVal>(Expression<Func<TEntity, TVal>> selector, string searchKey)
        where TVal : struct, IConvertible
    {
        var val = (TVal)Convert.ChangeType(searchKey, typeof(TVal));
        return ExpressionHelper.CreateEqualExpression(selector, val);
    }

    public static Expression<Func<TEntity, bool>> Between<TEntity, Tval>(Expression<Func<TEntity, Tval>> selector, string searchKey)
    {
        return Between<TEntity>(selector, searchKey);
    }

    public static Expression<Func<TEntity, bool>> Between<TEntity>(LambdaExpression propertySelector, string searchKey)
    {
        string[] dates = searchKey.Split(',');
        DateTime? fromDate = TryParse(dates[0]);
        DateTime? toDate = dates.Length > 1 ? TryParse(dates[1]) : null;

        return BetweenDates<TEntity>(propertySelector, fromDate, toDate);

        static DateTime? TryParse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            return DateTimeHelpers.TryParse(s, "yyyy-MM-dd") ?? DateTimeHelpers.TryParse(s, "dd/MM/yyyy");
        }
    }

    public static Expression<Func<TEntity, bool>> InList<TEntity>(LambdaExpression selector, string key)
    {
        var values = ConvertSearchKeyToList(key, selector.ReturnType);
        return CreateContainsExpression<TEntity, bool>(selector, values);
    }

    public static Expression<Func<TEntity, bool>> InList<TEntity, Tval>(Expression<Func<TEntity, Tval>> selector, string key)
    {
        return InList<TEntity>(selector, key);
    }

    public static IEnumerable<object> ConvertSearchKeyToList(string searchKey, Type sourceType)
    {
        var typeConverter = System.ComponentModel.TypeDescriptor.GetConverter(sourceType);
        var values = from item in searchKey.Split(',')
                     where !string.IsNullOrEmpty(item)
                     select typeConverter.ConvertFromString(item);

        return values;
    }

    public static IEnumerable<T> ConvertSearchKeyToList<T>(string searchKey)
        => ConvertSearchKeyToList(searchKey, typeof(T)).OfType<T>();

    public static Expression<Func<TEntity, bool>> Contains<TEntity>(LambdaExpression selector, string s)
    {
        Expression expression;

        if (selector.ReturnType == typeof(string))
        {
            expression = selector.Body;
        }
        else
        {
            var toStrMethod = typeof(object).GetMethod(nameof(ToString), Array.Empty<Type>());
            expression = Expression.Call(selector.Body, toStrMethod);
        }
        var containsExpression = typeof(string).GetMethod(
            nameof(string.Contains),
            new Type[] { typeof(string) });

        var argExpression = Expression.Constant(s);
        var body = Expression.Call(expression, containsExpression, argExpression);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(body, selector.Parameters[0]);
        return lambda;
    }

    public static LambdaExpression GetMemberExpression<TEntity>(string propertyName)
    {
        var splited = propertyName.Split('.');
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        Expression memberExpression = parameter;

        for (var i = 0; i < splited.Length; i++)
        {
            var name = splited[i];
            memberExpression = Expression.Property(memberExpression, name);
        }

        return Expression.Lambda(memberExpression, parameter);
    }

    private static Expression<Func<TObj, bool>> CreateContainsExpression<TObj, TVal>(LambdaExpression selector, IEnumerable values)
    {
        var genericCustMethod = EnumerableCust.Value.MakeGenericMethod(selector.ReturnType);
        var genericValues = genericCustMethod.Invoke(null, new object[] { values });

        var listType = typeof(List<>).MakeGenericType(selector.ReturnType);
        var list = Activator.CreateInstance(listType, genericValues);

        var ienumerableType = typeof(List<>).MakeGenericType(selector.ReturnType);
        var constantExpression = Expression.Constant(list, ienumerableType);

        MethodInfo genericMethod = EnumerableContains.Value.MakeGenericMethod(new[] { selector.ReturnType });

        var fieldExpression = selector.Body;
        var body = Expression.Call(null, genericMethod, constantExpression, fieldExpression);
        return Expression.Lambda<Func<TObj, bool>>(body, selector.Parameters[0]);
    }

    private static Expression<Func<TSource, bool>> BetweenDates<TSource>(
            LambdaExpression selector,
            DateTime? lower,
            DateTime? higher)
    {
        Expression body = selector.Body;

        Expression lowerBound = null;
        if (lower.HasValue)
        {
            lowerBound = Expression.GreaterThanOrEqual(
                body,
                Expression.Constant(lower.Value, typeof(DateTime)));
        }

        Expression upperBound = null;
        if (higher.HasValue)
        {
            higher = higher.Value.AddDays(1);

            upperBound = Expression.LessThan(
                body,
                Expression.Constant(higher.Value, typeof(DateTime)));
        }

        Expression exp = CreateBetweenExpession(lowerBound, upperBound);

        return Expression.Lambda<Func<TSource, bool>>(exp, selector.Parameters);
    }

    public static Expression CreateBetweenExpession(Expression lowerBound, Expression upperBound)
    {
        if (lowerBound == null && upperBound == null)
        {
            return Expression.Constant(true);
        }
        else if (lowerBound != null && upperBound == null)
        {
            return lowerBound;
        }
        else if (lowerBound == null && upperBound != null)
        {
            return upperBound;
        }
        else
        {
            return Expression.AndAlso(lowerBound, upperBound);
        }
    }
}
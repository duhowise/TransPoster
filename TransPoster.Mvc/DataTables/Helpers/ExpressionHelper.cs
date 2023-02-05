using System.Linq.Expressions;
using System.Reflection;

namespace TransPoster.Mvc.DataTables.Helpers;

public static class ExpressionHelper
{
    public static Expression<Func<TObj, bool>> CreateContainsExpression<TObj, TVal>(Expression<Func<TObj, TVal>> selector, IEnumerable<TVal> values)
    {
        var constantExpression = Expression.Constant(values);

        var containsMethod = typeof(Enumerable).GetMethods()
            .Where(method => method.Name == nameof(Enumerable.Contains))
            .Where(method => method.GetParameters().Length == 2)
            .Single();

        MethodInfo genericMethod = containsMethod.MakeGenericMethod(new[] { typeof(TVal) });

        var fieldExpression = selector.Body;
        var body = Expression.Call(null, genericMethod, constantExpression, fieldExpression);
        return Expression.Lambda<Func<TObj, bool>>(body, selector.Parameters[0]);
    }

    public static Expression<Func<TEntity, bool>> CreateStringConaintsExpression<TEntity>(LambdaExpression selector, string value)
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

        var argExpression = Expression.Constant(value);
        var body = Expression.Call(expression, containsExpression, argExpression);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(body, selector.Parameters[0]);
        return lambda;
    }

    public static Expression<Func<TObj, bool>> CreateEqualExpression<TObj>(LambdaExpression selector, object value)
    {
        Expression body = selector.Body;

        var expression = Expression.Equal(
                body,
                Expression.Constant(value, selector.ReturnType));

        return Expression.Lambda<Func<TObj, bool>>(expression, selector.Parameters);
    }

    public static Expression<Func<TObj, bool>> CreateEqualExpression<TObj, TVal>(Expression<Func<TObj, TVal>> selector, TVal value)
    {
        Expression body = selector.Body;

        var expression = Expression.Equal(
                body,
                Expression.Constant(value, typeof(TVal)));

        return Expression.Lambda<Func<TObj, bool>>(expression, selector.Parameters);
    }

    public static Expression<Func<TSource, bool>> Between<TSource, TKey>(
            Expression<Func<TSource, TKey?>> selector,
            TKey? lower,
            TKey? higher)
        where TKey : struct, IComparable
    {
        Expression body = selector.Body;

        Expression lowerBound = null;
        if (lower.HasValue)
        {
            lowerBound = Expression.GreaterThanOrEqual(
                body,
                Expression.Constant(lower, typeof(TKey?)));
        }

        Expression upperBound = null;
        if (higher.HasValue)
        {
            upperBound = Expression.LessThanOrEqual(
                body,
                Expression.Constant(higher, typeof(TKey?)));
        }

        Expression exp;

        if (lowerBound == null && upperBound == null)
        {
            exp = Expression.Constant(true);
        }
        else if (lowerBound != null && upperBound == null)
        {
            exp = lowerBound;
        }
        else if (lowerBound == null && upperBound != null)
        {
            exp = upperBound;
        }
        else
        {
            exp = Expression.AndAlso(lowerBound, upperBound);
        }

        return Expression.Lambda<Func<TSource, bool>>(exp, selector.Parameters);
    }

    public static Expression<Func<TEntity, bool>> CreateOrExpression<TEntity>(Expression<Func<TEntity, bool>> exp1, Expression<Func<TEntity, bool>> exp2)
    {
        return CreateOrExpression<TEntity>(new LambdaExpression[] { exp1, exp2 });
    }

    public static Expression<Func<TEntity, bool>> CreateAndExpressionSafe<TEntity>(Expression<Func<TEntity, bool>> exp1, Expression<Func<TEntity, bool>> exp2)
    {
        if (exp1 is null && exp2 is null)
        {
            throw new ArgumentNullException();
        }

        if (exp1 is null || exp2 is null)
        {
            return exp1 ?? exp2;
        }

        return CreateAndExpression(exp1, exp2);
    }

    public static Expression<Func<TEntity, bool>> CreateAndExpression<TEntity>(Expression<Func<TEntity, bool>> exp1, Expression<Func<TEntity, bool>> exp2)
    {
        Expression orExpression = Expression.AndAlso(exp1.Body, exp2.Body);

        var parameterExpression = Expression.Parameter(typeof(TEntity));
        var replacer = new ParameterReplacer(parameterExpression);
        orExpression = replacer.Visit(orExpression);


        return Expression.Lambda<Func<TEntity, bool>>(orExpression, parameterExpression);
    }

    public static Expression<Func<TEntity, bool>> CreateOrExpression<TEntity>(params LambdaExpression[] expression)
    {
        switch (expression.Length)
        {
            case 0:
                throw new ArgumentException(nameof(expression));
            case 1:
                return Expression.Lambda<Func<TEntity, bool>>(expression[0].Body, expression[0].Parameters);

            default:
                Expression orExpression = Expression.OrElse(expression[0].Body, expression[1].Body);

                for (var i = 2; i < expression.Length; i++)
                {
                    orExpression = Expression.OrElse(orExpression, expression[i].Body);
                }

                var parameterExpression = Expression.Parameter(typeof(TEntity));
                var replacer = new ParameterReplacer(parameterExpression);
                orExpression = replacer.Visit(orExpression);


                return Expression.Lambda<Func<TEntity, bool>>(orExpression, parameterExpression);
        }
    }

    private sealed class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression parameter;

        internal ParameterReplacer(ParameterExpression parameter) => this.parameter = parameter;

        protected override Expression VisitParameter
            (ParameterExpression node)
            => parameter;
    }
}
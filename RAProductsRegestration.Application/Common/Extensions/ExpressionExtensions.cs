using System.Linq.Expressions;

namespace RAProductsRegestration.Application.Common.Extensions;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(
            typeof(T),
            "x");

        var leftBody = new ReplaceParameterVisitor(
            left.Parameters[0],
            parameter)
            .Visit(left.Body);

        var rightBody = new ReplaceParameterVisitor(
            right.Parameters[0],
            parameter)
            .Visit(right.Body);

        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                leftBody!,
                rightBody!),
            parameter);
    }

    private sealed class ReplaceParameterVisitor(
        ParameterExpression oldParameter,
        ParameterExpression newParameter)
        : ExpressionVisitor
    {
        protected override Expression VisitParameter(
            ParameterExpression node)
        {
            return node == oldParameter
                ? newParameter
                : base.VisitParameter(node);
        }
    }
}
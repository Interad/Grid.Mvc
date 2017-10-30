using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using GridMvc.Filtering.Types;

namespace GridMvc.Filtering
{
    public class CustomExpressionFilter<T> : IColumnFilter<T>
    {
        private Expression<Func<T, string, bool>> _expression;
        //private Expression<Func<T, GridFilterType, string, bool>> _expression;

        public CustomExpressionFilter(Expression<Func<T, string, bool>> expression)
        {
            _expression = expression;
        }

        public IQueryable<T> ApplyFilter(IQueryable<T> items, ColumnFilterValue value)
        {
            var expr = ConvertExpression(_expression, value.FilterValue);
            if (expr == null)
                return items;

            return items.Where(expr);
        }

        // thanks StackOverflow: https://stackoverflow.com/a/3530666
        private static Expression<Func<T1, TResult>> ConvertExpression<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> source, T2 argument)
        {
            Expression arg2 = Expression.Constant(argument, typeof(T2));
            Expression newBody = new Rewriter(source.Parameters[1], arg2).Visit(source.Body);
            return Expression.Lambda<Func<T1, TResult>>(newBody, source.Parameters[0]);
        }

        private class Rewriter : ExpressionVisitor
        {
            private readonly Expression candidate_;
            private readonly Expression replacement_;

            public Rewriter(Expression candidate, Expression replacement)
            {
                candidate_ = candidate;
                replacement_ = replacement;
            }

            public override Expression Visit(Expression node)
            {
                return node == candidate_ ? replacement_ : base.Visit(node);
            }
        }
    }
}

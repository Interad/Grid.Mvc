using System;
using System.Linq;
using System.Linq.Expressions;
using GridMvc.Filtering.Types;

namespace GridMvc.Filtering
{
    /// <summary>
    ///     Default Grid.Mvc filter. Provides logic for filtering items collection.
    /// </summary>
    internal class ComplexExpressionColumnFilter<T, TData> : IColumnFilter<T>
    {
        private readonly Expression<Func<T, TData>> _expression;
        private readonly FilterTypeResolver _typeResolver = new FilterTypeResolver();

        public ComplexExpressionColumnFilter(Expression<Func<T, TData>> expression)
        {
            _expression = expression;
        }

        #region IColumnFilter<T> Members

        public IQueryable<T> ApplyFilter(IQueryable<T> items, ColumnFilterValue value)
        {
            if (value == ColumnFilterValue.Null)
                throw new ArgumentNullException("value");

            Expression<Func<T, bool>> expr = GetFilterExpression(_expression, value);
            if (expr == null)
                return items;
            return items.Where(expr);
        }

        #endregion

        private Expression<Func<T, bool>> GetFilterExpression(Expression<Func<T, TData>> expr, ColumnFilterValue value)
        {
            //detect nullable
            var isNullable = expr.ReturnType.IsGenericType &&
                             expr.ReturnType.GetGenericTypeDefinition() == typeof (Nullable<>);
            //get target type:
            var targetType = isNullable ? Nullable.GetUnderlyingType(expr.ReturnType) : expr.ReturnType;

            var filterType = _typeResolver.GetFilterType(targetType);

            //build expression to filter collection:
            var entityParam = _expression.Parameters[0];
            //support nullable types:
            var firstExpr = isNullable
                ? Expression.Property(_expression.Body, expr.ReturnType.GetProperty("Value"))
                : _expression.Body;

            var binaryExpression = filterType.GetFilterExpression(firstExpr, value.FilterValue, value.FilterType);
            if (binaryExpression == null) return null;

            if (targetType == typeof (string))
            {
                //check for strings, they may be NULL
                //It's ok for ORM, but throw exception in linq to objects. Additional check string on null
                Expression nullExpr = Expression.NotEqual(_expression.Body, Expression.Constant(null));
                binaryExpression = Expression.AndAlso(nullExpr, binaryExpression);
            }
            else if (isNullable)
            {
                //add additional filter condition for check items on NULL with invoking "HasValue" method.
                //for example: result of this expression will like - c=> c.HasValue && c.Value = 3
                var hasValueExpr = Expression.Property(_expression.Body, expr.ReturnType.GetProperty("HasValue"));
                binaryExpression = Expression.AndAlso(hasValueExpr, binaryExpression);
            }
            //return filter expression
            return Expression.Lambda<Func<T, bool>>(binaryExpression, entityParam);
        }
    }
}
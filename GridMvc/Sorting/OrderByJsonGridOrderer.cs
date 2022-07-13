using GridMvc.Utility;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Sorting
{
    /// <summary>
    ///     Object applies order (OrderBy, OrderByDescending) for items collection
    /// </summary>
    internal class OrderByJsonGridOrderer<T, TDataType> : IColumnOrderer<T>
    {
        private readonly Expression<Func<T, string>> _expression;
        private readonly string _propertyName;

        public OrderByJsonGridOrderer(Expression<Func<T, string>> expression, string propertyName)
        {
            _expression = expression;
            _propertyName = propertyName;
        }

        #region IColumnOrderer<T> Members

        public IQueryable<T> ApplyOrder(IQueryable<T> items)
        {
            return ApplyOrder(items, GridSortDirection.Ascending);
        }

        public IQueryable<T> ApplyOrder(IQueryable<T> items, GridSortDirection direction)
        {
            ParameterExpression entityParam = _expression.Parameters[0];
            var expressionBody = _expression.Body;
            var methodInfoJsonValue = typeof(CustomFunction).GetMethod(nameof(CustomFunction.JsonValue), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            var jsonValueExpression = Expression.Call(methodInfoJsonValue, expressionBody, Expression.Constant($"$.{_propertyName}"));
            var convertedValue = DbStringConverter.GetConvertToTypeExpression(typeof(TDataType), jsonValueExpression);
            var orderExpression = Expression.Lambda<Func<T, TDataType>>(convertedValue, entityParam);

            switch (direction)
            {
                case GridSortDirection.Ascending:
                    return items.OrderBy(orderExpression);
                case GridSortDirection.Descending:
                    return items.OrderByDescending(orderExpression);
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        #endregion
    }
}
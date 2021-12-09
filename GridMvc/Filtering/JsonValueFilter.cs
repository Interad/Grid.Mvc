using GridMvc.Filtering.Types;
using GridMvc.Utility;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Filtering
{
    public class JsonValueFilter<T, TData> : IColumnFilter<T>
    {
        private readonly Expression<Func<T, TData>> _expression;
        private readonly FilterTypeResolver _typeResolver = new FilterTypeResolver();
        private readonly string _propertyName;

        public JsonValueFilter(Expression<Func<T, TData>> expression, string propertyName)
        {
            _expression = expression;
            _propertyName = propertyName;
        }

        #region IColumnFilter<T> Members

        public IQueryable<T> ApplyFilter(IQueryable<T> items, ColumnFilterValue value)
        {
            if (value == ColumnFilterValue.Null)
                throw new ArgumentNullException("value");

            var pi = (PropertyInfo)((MemberExpression)_expression.Body).Member;
            Expression<Func<T, bool>> expr = GetFilterExpression(pi, value);
            if (expr == null)
                return items;
            return items.Where(expr);
        }

        #endregion

        private Expression<Func<T, bool>> GetFilterExpression(PropertyInfo pi, ColumnFilterValue value)
        {
            //detect nullable
            bool isNullable = pi.PropertyType.IsGenericType &&
                              pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            //get target type:
            Type targetType = isNullable ? Nullable.GetUnderlyingType(pi.PropertyType) : pi.PropertyType;

            IFilterType filterType = (IFilterType)new JsonFilterType();

            //build expression to filter collection:
            ParameterExpression entityParam = _expression.Parameters[0];
            //support nullable types:
            Expression firstExpr = isNullable
                                       ? Expression.Property(_expression.Body, pi.PropertyType.GetProperty("Value"))
                                       : _expression.Body;
            
            // call DBFunction for JSON_VALUE
            MethodInfo methodInfoJsonValue = typeof(CustomFunction).GetMethod("JsonValue", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            Expression jsonValueExpression = Expression.Call(methodInfoJsonValue, firstExpr, Expression.Constant($"$.{value.ColumnName}"));

            Expression binaryExpression = filterType.GetFilterExpression(jsonValueExpression, value.FilterValue, value.FilterType);
            if (binaryExpression == null)
                return null;

            if (targetType == typeof(string))
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
                MemberExpression hasValueExpr = Expression.Property(_expression.Body,
                                                                    pi.PropertyType.GetProperty("HasValue"));
                binaryExpression = Expression.AndAlso(hasValueExpr, binaryExpression);
            }
            //return filter expression
            return Expression.Lambda<Func<T, bool>>(binaryExpression, entityParam);
        }
    }
}

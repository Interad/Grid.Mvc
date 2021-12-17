﻿using GridMvc.Filtering.Types;
using GridMvc.Utility;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Filtering
{
    public class JsonValueFilter<T, TDataType> : IColumnFilter<T>
    {
        private readonly Expression<Func<T, string>> _expression;
        private readonly FilterTypeResolver _typeResolver = new FilterTypeResolver();
        private readonly string _propertyName;

        public JsonValueFilter(Expression<Func<T, string>> expression, string propertyName)
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
            var type = typeof(TDataType);

            //detect nullable
            bool isNullable = pi.PropertyType.IsGenericType &&
                              pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            //get target type:
            Type targetType = isNullable ? Nullable.GetUnderlyingType(pi.PropertyType) : pi.PropertyType;

            IFilterType filterType = _typeResolver.GetFilterType(type);

            //build expression to filter collection:
            ParameterExpression entityParam = _expression.Parameters[0];
            //support nullable types:
            Expression firstExpr = _expression.Body;
            
            // call DBFunction for JSON_VALUE
            MethodInfo methodInfoJsonValue = typeof(CustomFunction).GetMethod("JsonValue", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            Expression jsonValueExpression = Expression.Call(methodInfoJsonValue, firstExpr, Expression.Constant($"$.{value.ColumnName}"));
            Expression convertedValue = GetConvertToTypeExpression(filterType.TargetType, jsonValueExpression);

            Expression binaryExpression = filterType.GetFilterExpression(convertedValue, value.FilterValue, value.FilterType);
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

        private Expression GetConvertToTypeExpression(Type targetType, Expression leftSide)
        {
            if (targetType == typeof(decimal))
            {
                return GetConvertToDecimal(leftSide);
            }
            else if (targetType == typeof(DateTime))
            {
                return GetConvertToDateTimeExpression(leftSide);
            }

            return leftSide;
        }

        private Expression GetConvertToDateTimeExpression(Expression leftSide)
        {
            MethodInfo methodInfoToDateTime = typeof(CustomFunction).GetMethod("ToDateTime2", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            Expression convertedValue = Expression.Call(methodInfoToDateTime, leftSide, Expression.Constant("yyyy-MM-dd HH:mm:ss"));
            return convertedValue;
        }

        private Expression GetConvertToDecimal(Expression leftSide)
        {
            MethodInfo methodInfoToDateTime = typeof(CustomFunction).GetMethod("ToDecimal", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            Expression convertedValue = Expression.Call(methodInfoToDateTime, leftSide);
            return convertedValue;
        }
    }
}

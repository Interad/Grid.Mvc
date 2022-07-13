using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GridMvc.Utility
{
    public static class DbStringConverter
    {
        public static Expression GetConvertToTypeExpression(Type targetType, Expression leftSide)
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

        private static Expression GetConvertToDateTimeExpression(Expression leftSide, string dateTimeFormat = null)
        {
            dateTimeFormat = dateTimeFormat ?? "yyyy-MM-dd HH:mm:ss";
            var methodInfoToDateTime = typeof(CustomFunction).GetMethod(nameof(CustomFunction.ToDateTime2), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            var convertedValue = Expression.Call(methodInfoToDateTime, leftSide, Expression.Constant(dateTimeFormat));
            return convertedValue;
        }

        private static Expression GetConvertToDecimal(Expression leftSide)
        {
            var methodInfoToDateTime = typeof(CustomFunction).GetMethod(nameof(CustomFunction.ToDecimal), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            var convertedValue = Expression.Call(methodInfoToDateTime, leftSide);
            return convertedValue;
        }
    }
}

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Filtering.Types
{
    /// <summary>
    ///     Object builds filter expressions for text (string) grid columns
    /// </summary>
    internal sealed class TextFilterType : FilterTypeBase
    {
        public override Type TargetType
        {
            get { return typeof (String); }
        }

        public override GridFilterType GetValidType(GridFilterType type)
        {
            switch (type)
            {
                case GridFilterType.Equals:
                case GridFilterType.Contains:
                case GridFilterType.StartsWith:
                case GridFilterType.EndsWidth:
                    return type;
                default:
                    return GridFilterType.Equals;
            }
        }

        public override object GetTypedValue(string value)
        {
            return value;
        }

        public override Expression GetFilterExpression(Expression leftExpr, string value, GridFilterType filterType)
        {
            //Custom implementation of string filter type. Case insensitive comparison.

            filterType = GetValidType(filterType);
            object typedValue = GetTypedValue(value);
            if (typedValue == null)
                return null; //incorrect filter value;

            Expression valueExpr = Expression.Constant(typedValue);
            Expression binaryExpression;
            switch (filterType)
            {
                case GridFilterType.Equals:
                    binaryExpression = GetCaseInsensitiveComparison(string.Empty, leftExpr, valueExpr);
                    break;
                case GridFilterType.Contains:
                    binaryExpression = GetCaseInsensitiveComparison("Contains", leftExpr, valueExpr);
                    break;
                case GridFilterType.StartsWith:
                    binaryExpression = GetCaseInsensitiveComparison("StartsWith", leftExpr, valueExpr);
                    break;
                case GridFilterType.EndsWidth:
                    binaryExpression = GetCaseInsensitiveComparison("EndsWith", leftExpr, valueExpr);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return binaryExpression;
        }

        private Expression GetCaseInsensitiveComparison(string methodName, Expression leftExpr, Expression rightExpr)
        {
            Type targetType = TargetType;
            //case insensitive comparison:
            MethodInfo miUpper = targetType.GetMethod("ToUpper", new Type[] {});
            MethodCallExpression upperValueExpr = Expression.Call(rightExpr, miUpper);
            MethodCallExpression upperFirstExpr = Expression.Call(leftExpr, miUpper);

            if (!string.IsNullOrEmpty(methodName))
            {
                MethodInfo mi = targetType.GetMethod(methodName, new[] {typeof (string)});
                if (mi == null)
                    throw new MissingMethodException("There is no method - " + methodName);
                return Expression.Call(upperFirstExpr, mi, upperValueExpr);
            }
            return Expression.Equal(upperFirstExpr, upperValueExpr);
        }
    }
}
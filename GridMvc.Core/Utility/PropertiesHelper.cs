using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace GridMvc.Core.Utility
{
    /// <summary>
    ///     Helper class for reflection operations
    /// </summary>
    internal static class PropertiesHelper
    {
        private const string PropertiesQueryStringDelimiter = ".";

        public static string BuildColumnNameFromMemberExpression(MemberExpression memberExpr)
        {
            var sb = new StringBuilder();
            Expression expr = memberExpr;
            while (true)
            {
                string piece = GetExpressionMemberName(expr, ref expr);
                if (string.IsNullOrEmpty(piece)) break;
                if (sb.Length > 0)
                    sb.Insert(0, PropertiesQueryStringDelimiter);
                sb.Insert(0, piece);
            }
            return sb.ToString();
        }

        private static string GetExpressionMemberName(Expression expr, ref Expression nextExpr)
        {
            if (expr is MemberExpression)
            {
                var memberExpr = (MemberExpression) expr;
                nextExpr = memberExpr.Expression;
                return memberExpr.Member.Name;
            }
            if (expr is BinaryExpression && expr.NodeType == ExpressionType.ArrayIndex)
            {
                var binaryExpr = (BinaryExpression) expr;
                string memberName = GetExpressionMemberName(binaryExpr.Left, ref nextExpr);
                if (string.IsNullOrEmpty(memberName))
                    throw new InvalidDataException("Cannot parse your column expression");
                return string.Format("{0}[{1}]", memberName, binaryExpr.Right);
            }
            return string.Empty;
        }

        public static PropertyInfo GetPropertyFromColumnName(string columnName, Type type,
                                                             out IEnumerable<PropertyInfo> propertyInfoSequence)
        {
            string[] properies = columnName.Split(new[] {PropertiesQueryStringDelimiter},
                                                  StringSplitOptions.RemoveEmptyEntries);
            if (!properies.Any())
            {
                propertyInfoSequence = null;
                return null;
            }
            PropertyInfo pi = null;
            var sequence = new List<PropertyInfo>();
            foreach (string propertyName in properies)
            {
                pi = type.GetProperty(propertyName);
                if (pi == null)
                {
                    propertyInfoSequence = null;
                    return null; //no match column
                }
                sequence.Add(pi);
                type = pi.PropertyType;
            }
            propertyInfoSequence = sequence;
            return pi;
        }

        public static Type GetUnderlyingType(Type type)
        {
            Type targetType;
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
            {
                targetType = Nullable.GetUnderlyingType(type);
            }
            else
            {
                targetType = type;
            }
            return targetType;
        }

        public static T GetAttribute<T>(this PropertyInfo pi) where T : Attribute
        {
            return pi.PropertyType.GetTypeInfo().GetCustomAttributes<T>().FirstOrDefault();
        }

        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetTypeInfo().GetCustomAttributes<T>(true).FirstOrDefault();
        }

        public static PropertyInfo GetProperty<T, T2>(Expression<Func<T, T2>> expression)
        {
            MemberExpression memberExpression = null;

            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                    memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
                    break;
                case ExpressionType.MemberAccess:
                    memberExpression = expression.Body as MemberExpression;
                    break;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("Not a member access", nameof(expression));
            }

            return memberExpression.Member as PropertyInfo;
        }

        public static string GetDisplayName<T, T2>(Expression<Func<T, T2>> titleField)
        {
            var propertyInfo = GetProperty(titleField);
            var displayAttribute = propertyInfo.GetAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                return displayAttribute.Name;
            }

            var displayNameAttribute = propertyInfo.GetAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null)
            {
                return displayNameAttribute.DisplayName;
            }

            return null;
        }
    }
}
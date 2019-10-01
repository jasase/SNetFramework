using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core.Helper
{
    public static class ExpressionHelper
    {
        public static MemberExpression GetMemberExpression<TEntity, TData>(Expression<Func<TEntity, TData>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression == null && memberExpression.Member.MemberType != MemberTypes.Property)
            {
                throw new ArgumentException(string.Format("Provided expression '{0}' does not select a property", propertySelector.ToString()));
            }

            return memberExpression;
        }

        public static Func<TEntity, TData> DetermineGetter<TEntity, TData>(MemberExpression memberExpression)
        {
            var property = (PropertyInfo)memberExpression.Member;
            var getMethod = property.GetGetMethod();

            var parameterT = Expression.Parameter(typeof(TEntity), "x");

            var newExpression =
                Expression.Lambda<Func<TEntity, TData>>(
                    Expression.Call(parameterT, getMethod),
                    parameterT
                );

            return newExpression.Compile();
        }

        public static Action<TEntity, TData> DetermineSetter<TEntity, TData>(MemberExpression memberExpression)
        {
            var property = (PropertyInfo)memberExpression.Member;
            var setMethod = property.GetSetMethod();

            var parameterT = Expression.Parameter(typeof(TEntity), "x");
            var parameterTProperty = Expression.Parameter(typeof(TData), "y");

            var newExpression =
                Expression.Lambda<Action<TEntity, TData>>(
                    Expression.Call(parameterT, setMethod, parameterTProperty),
                    parameterT,
                    parameterTProperty
                );

            return newExpression.Compile();
        }
    }
}

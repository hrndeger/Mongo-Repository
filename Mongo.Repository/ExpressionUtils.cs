using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Mongo.Repository
{
    public static class ExpressionUtils
    {
        private const string EXPRESSION_BODY_MUST_BE_MEMBEREXPRESSION =
           "Expression body must be MemberExpression, MethodCallExpression or UnaryExpression";

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <typeparam name="TProp">The type of the property.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public static string GetMemberName<TType, TProp>(Expression<Func<TType, TProp>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            MemberInfo memberInfo = GetMemberInfo(expression);

            return memberInfo.Name;
        }

        /// <summary>
        /// Gets the member information.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <typeparam name="TMember">The type of the member.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public static MemberInfo GetMemberInfo<TType, TMember>(Expression<Func<TType, TMember>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            return GetMemberInfo((LambdaExpression)expression);
        }

        /// <summary>
        /// Gets the member information.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static MemberInfo GetMemberInfo(LambdaExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            MemberExpression body = expression.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    body = unaryExpression.Operand as MemberExpression;
                }
            }

            if (body == null)
            {
                MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;
                if (methodCallExpression != null)
                {
                    return methodCallExpression.Method;
                }
            }

            if (body == null)
            {
                throw new InvalidOperationException(EXPRESSION_BODY_MUST_BE_MEMBEREXPRESSION);
            }

            return body.Member;
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public static string GetMemberName(LambdaExpression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            MemberInfo memberInfo = GetMemberInfo(expression);

            return memberInfo.Name;
        }
    }
}
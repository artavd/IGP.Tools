using JetBrains.Annotations;

namespace SBL.Common
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    public static class Contract
    {
        [DebuggerStepThrough]
        [AssertionMethod]
        public static void ArgumentIsNotNull<T>(
            [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T value,
            Expression<Func<T>> valueName,
            [CallerMemberName] string caller = "")
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(
                    $"Argument '{valueName.GetFieldName()}' is null in '{caller}' method");
            }
        }

        [DebuggerStepThrough]
        [AssertionMethod]
        public static void ArgumentSatisfied<T>(
            T value,
            Expression<Func<T>> valueName,
            Func<T, bool> checker,
            [CallerMemberName] string caller = "")
        {
            if (!checker(value))
            {
                throw new ArgumentException(
                    $"Argument '{valueName.GetFieldName()}' doesn't satisfied expression in '{caller}' method");
            }
        }

        [DebuggerStepThrough]
        [AssertionMethod]
        public static void IsNotNull<T>(
            [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T value,
            Func<string> messageProvider = null,
            [CallerMemberName] string caller = "") where T : class
        {
            if (value == null)
            {
                string message = messageProvider == null ?
                    $"Object is null in '{caller}' method" :
                    messageProvider();

                throw new ContractException(message);
            }
        }

        [DebuggerStepThrough]
        [AssertionMethod]
        public static void IsTrue(
            [AssertionCondition(AssertionConditionType.IS_TRUE)] bool value,
            Func<string> messageProvider = null,
            [CallerMemberName] string caller = "")
        {
            if (!value)
            {
                string message = messageProvider == null ?
                    $"Expression should be true but was false in '{caller}' method" :
                    messageProvider();

                throw new ContractException(message);
            }
        }

        [DebuggerStepThrough]
        [AssertionMethod]
        public static void OfType<T>(
            object value,
            Func<string> messageProvider = null,
            [CallerMemberName] string caller = "")
        {
            if (!(value is T))
            {
                string message = messageProvider == null ?
                    $"Object should be of type '{typeof (T)}' but was '{value.GetType()}' in '{caller}' method"
                    :
                    messageProvider();

                throw new ContractException(message);
            }
        }

        private static string GetFieldName(this LambdaExpression expression)
        {
            var ex = expression.Body as MemberExpression;
            return ex?.Member.Name ?? "<unknown name>";
        }
    }
}
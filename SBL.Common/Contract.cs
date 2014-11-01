namespace SBL.Common
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    public static class Contract
    {
        [DebuggerStepThrough]
        public static void ArgumentIsNotNull<T>(
            T value,
            Expression<Func<T>> valueName,
            [CallerMemberName] string caller = "")
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(
                    string.Format(
                        "Argument '{0}' is null in '{1}' method",
                        valueName.GetFieldName(),
                        caller));
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentSatisfied<T>(
            T value,
            Expression<Func<T>> valueName,
            Func<T, bool> checker,
            [CallerMemberName] string caller = "")
        {
            if (!checker(value))
            {
                throw new ArgumentException(
                    string.Format(
                        "Argument '{0}' doesn't satisfied expression in '{1}' method",
                        valueName.GetFieldName(),
                        caller));
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNull<T>(
            T value,
            Func<string> messageProvider = null,
            [CallerMemberName] string caller = "") where T : class
        {
            if (value == null)
            {
                string message = messageProvider == null ?
                    string.Format("Object is null in '{0}' method", caller) :
                    messageProvider();

                throw new ContractException(message);
            }
        }

        [DebuggerStepThrough]
        public static void IsTrue(
            bool value,
            Func<string> messageProvider = null,
            [CallerMemberName] string caller = "")
        {
            if (!value)
            {
                string message = messageProvider == null ?
                    string.Format("Expression should be true but was false in '{0}' method", caller) :
                    messageProvider();

                throw new ContractException(message);
            }
        }

        [DebuggerStepThrough]
        public static void OfType<T>(
            object value,
            Func<string> messageProvider = null,
            [CallerMemberName] string caller = "")
        {
            if (!(value is T))
            {
                string message = messageProvider == null ?
                    string.Format(
                        "Object should be of type '{0}' but was '{1}' in '{2}' method",
                        typeof (T).ToString(),
                        value.GetType().ToString(),
                        caller) :
                    messageProvider();

                throw new ContractException(message);
            }
        }

        private static string GetFieldName(this LambdaExpression expression)
        {
            var ex = expression.Body as MemberExpression;
            return ex == null ? "<unknown name>" : ex.Member.Name;
        }
    }
}
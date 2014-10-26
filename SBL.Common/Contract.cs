namespace SBL.Common
{
    using System;
    using System.Linq.Expressions;

    public static class Contract
    {
        public static void ArgumentIsNotNull<T>(T value, Expression<Func<T>> valueName) where T : class
        {
            if (value == null)
            {
                throw new ContractException(string.Format("Argument '{0}' is null", valueName.GetFieldName()));
            }
        }

        public static void IsNotNull<T>(T value, Func<string> messageProvider = null) where T : class
        {
            if (value == null)
            {
                string message = messageProvider == null ? string.Format("Object is null") : messageProvider();

                throw new ContractException(message);
            }
        }

        public static void IsTrue(bool value, Func<string> messageProvider = null)
        {
            if (!value)
            {
                string message = messageProvider == null ?
                    string.Format("Expression should be true but was false") :
                    messageProvider();

                throw new ContractException(message);
            }
        }

        public static void OfType<T>(object value, Func<string> messageProvider = null)
        {
            if (!(value is T))
            {
                string message = messageProvider == null ?
                    string.Format(
                        "Object should be of type '{0}' but was '{1}'",
                        typeof (T).ToString(),
                        value.GetType().ToString()) :
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

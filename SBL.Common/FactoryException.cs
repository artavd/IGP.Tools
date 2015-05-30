namespace SBL.Common
{
    using System;
    using System.Text;

    using SBL.Common.Annotations;

    public class FactoryException : Exception
    {
        private readonly object _parameters;
        private readonly Type _factoryType;
        private readonly Type _createdType;

        public FactoryException(
            [NotNull] Type factoryType,
            [NotNull] Type createdType,
            [CanBeNull] string message = null,
            [CanBeNull] object parameters = null,
            [CanBeNull] Exception innerException = null)
            : base(message, innerException)
        {
            _parameters = parameters;
            _factoryType = factoryType;
            _createdType = createdType;
        }

        public override string Message
        {
            get
            {
                var messageBuilder = new StringBuilder(string.Format(
                    "{0}{1}Error occurs while creating '{2}' object with '{3}' factory.",
                    base.Message,
                    Environment.NewLine,
                    _createdType.Name,
                    _factoryType.Name));

                if (_parameters != null)
                {
                    messageBuilder.AppendLine();
                    messageBuilder.AppendFormat(
                        " - '{0}' parameters have been used for creating.",
                        _parameters);
                }

                if (InnerException != null)
                {
                    messageBuilder.AppendLine();
                    messageBuilder.AppendFormat(
                        " - Inner Exception:{2}   '{0}' of '{1}' type",
                        InnerException.Message,
                        InnerException.GetType(),
                        Environment.NewLine);
                }

                return messageBuilder.ToString();
            }
        }
    }
}
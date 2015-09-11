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
            Contract.ArgumentIsNotNull(factoryType, () => factoryType);
            Contract.ArgumentIsNotNull(createdType, () => createdType);

            _factoryType = factoryType;
            _createdType = createdType;

            _parameters = parameters;
        }

        public override string Message
        {
            get
            {
                var messageBuilder = new StringBuilder(base.Message);
                messageBuilder.AppendLine();
                messageBuilder.Append(
                    $"Error occurs while creating '{_createdType.Name}' object with '{_factoryType.Name}' factory.");

                if (_parameters != null)
                {
                    messageBuilder.AppendLine();
                    messageBuilder.Append($" - '{_parameters}' parameters have been used for creating.";
                }

                if (InnerException != null)
                {
                    messageBuilder.AppendLine();
                    messageBuilder.Append(" - Inner Exception:");
                    messageBuilder.AppendLine();
                    messageBuilder.Append($"   '{InnerException.Message}' of '{InnerException.GetType()}' type");
                }

                return messageBuilder.ToString();
            }
        }
    }
}
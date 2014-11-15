namespace SBL.Common
{
    using System;
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
                var message = string.Format(
                    "{0}{1}Error occurs while creating '{2}' object with '{3}' factory.",
                    base.Message,
                    Environment.NewLine,
                    _factoryType.Name,
                    _createdType.Name);

                if (_parameters != null)
                {
                    var additionalString = string.Format(
                        "'{0}' parameters have been used for creating.",
                        _parameters.ToString());

                    message = string.Concat(message, Environment.NewLine, additionalString);
                }

                return message;
            }
        }
    }
}
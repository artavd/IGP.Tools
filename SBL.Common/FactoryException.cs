namespace SBL.Common
{
    using System;

    public class FactoryException<TFactory, TCreated> : Exception
    {
        private readonly object _parameters;

        public FactoryException(string message = null, object parameters = null, Exception innerException = null)
            : base(message, innerException)
        {
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
                    typeof (TCreated).Name,
                    typeof (TFactory).Name);

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
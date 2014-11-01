namespace SBL.Common
{
    using System;
    using SBL.Common.Annotations;

    public class ContractException : ApplicationException
    {
        public ContractException([NotNull] string message) : base(message)
        {
            Contract.ArgumentIsNotNull(message, () => message);
        }
    }
}
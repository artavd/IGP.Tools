namespace SBL.Common
{
    using System;

    public class ContractException : ApplicationException
    {
        public ContractException(string message) : base (message) { }
    }
}
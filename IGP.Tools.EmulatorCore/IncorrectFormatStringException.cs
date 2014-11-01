namespace IGP.Tools.EmulatorCore
{
    using System;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class IncorrectFormatStringException : Exception
    {
        public IncorrectFormatStringException([NotNull] string message) : base(message)
        {
            Contract.ArgumentIsNotNull(message, () => message);
        }
    }
}
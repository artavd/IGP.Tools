namespace IGP.Tools.EmulatorCore
{
    using System;

    internal sealed class IncorrectFormatStringException : Exception
    {
        public IncorrectFormatStringException(string message) : base(message) { }
    }
}
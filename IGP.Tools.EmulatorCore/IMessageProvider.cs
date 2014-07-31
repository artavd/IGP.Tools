namespace IGP.Tools.EmulatorCore
{
    using System;

    internal interface IMessageProvider
    {
        TimeSpan Interval { get; }

        string GetNextMessage();
    }
}
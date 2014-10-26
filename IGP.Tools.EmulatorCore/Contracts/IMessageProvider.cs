namespace IGP.Tools.EmulatorCore.Contracts
{
    using System;

    public interface IMessageProvider
    {
        TimeSpan Interval { get; }

        string GetNextMessage();
    }
}
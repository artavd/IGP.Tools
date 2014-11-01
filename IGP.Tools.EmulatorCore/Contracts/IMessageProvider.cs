namespace IGP.Tools.EmulatorCore.Contracts
{
    using System;
    using SBL.Common.Annotations;

    public interface IMessageProvider
    {
        TimeSpan Interval { get; }

        [NotNull]
        string GetNextMessage();
    }
}
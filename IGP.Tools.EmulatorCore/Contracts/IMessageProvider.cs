namespace IGP.Tools.EmulatorCore
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
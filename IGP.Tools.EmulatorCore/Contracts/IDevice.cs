namespace IGP.Tools.EmulatorCore
{
    using System;
    using System.Collections.Generic;
    using SBL.Common.Annotations;

    public interface IDevice : IDisposable
    {
        [NotNull]
        string Name { get; }

        [NotNull] 
        IEnumerable<IObservable<byte[]>> Messages { get; }
    }
}
namespace IGP.Tools.EmulatorCore.Contracts
{
    using SBL.Common.Annotations;

    public interface IValueProvider
    {
        [NotNull]
        string Name { get; }

        [NotNull]
        string GetNextValue();
    }
}
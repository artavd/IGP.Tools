namespace IGP.Tools.EmulatorCore
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
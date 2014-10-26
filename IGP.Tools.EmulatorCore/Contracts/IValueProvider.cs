namespace IGP.Tools.EmulatorCore.Contracts
{
    public interface IValueProvider
    {
        string Name { get; }

        string GetNextValue();
    }
}
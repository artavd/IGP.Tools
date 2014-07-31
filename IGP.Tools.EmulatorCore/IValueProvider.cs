namespace IGP.Tools.EmulatorCore
{
    public interface IValueProvider
    {
        string Name { get; }

        string GetNextValue();
    }
}
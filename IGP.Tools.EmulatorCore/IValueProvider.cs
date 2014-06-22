namespace IGP.Tools.EmulatorCore
{
    public interface IValueProvider
    {
        string Name { get; set; }

        string GetNextValue();
    }
}
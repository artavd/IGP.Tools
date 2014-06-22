namespace IGP.Tools.EmulatorCore
{
    public interface IMessageProvider
    {
        string Name { get; set; }

        string FormatString { get; set; }

        IValueProvider[] Values { get; }

        string GetNextMessage();
    }
}
namespace IGP.Tools.EmulatorCore.Implementation
{
    using IGP.Tools.EmulatorCore.Contracts;

    public class VoidValueProvider : IValueProvider
    {
        public string Name { get; set; }

        public VoidValueProvider()
        {
            Name = "<Void Value Provider>";
        }

        public string GetNextValue()
        {
            return "<null>";
        }
    }
}
namespace IGP.Tools.EmulatorCore.Implementation
{
    public sealed class VoidValueProvider : IValueProvider
    {
        public string Name { get; set; }

        public VoidValueProvider()
        {
            Name = "<Void Value Provider>";
        }

        public string GetNextValue() => "<null>";
    }
}
namespace IGP.Tools.EmulatorCore
{
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
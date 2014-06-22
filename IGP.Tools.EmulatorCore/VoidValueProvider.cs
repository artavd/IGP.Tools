namespace IGP.Tools.EmulatorCore
{
    public class VoidValueProvider : IValueProvider
    {
        public string GetNextValue()
        {
            return "<null>";
        }
    }
}
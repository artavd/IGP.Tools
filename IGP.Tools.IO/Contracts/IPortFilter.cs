namespace IGP.Tools.IO.Contracts
{
    public interface IPortFilter
    {
        bool IsEnabled { get; set; }

        byte[] Filter(byte[] data);
    }
}
namespace IGP.Tools.IO.Contracts
{
    public interface IPortFilter
    {
        byte[] Filter(byte[] data);
    }
}
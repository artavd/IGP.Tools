namespace IGP.Tools.EmulatorCore.Implementation
{
    using System.Text;

    internal sealed class AsciiEncoder : IEncoder
    {
        public byte[] Encode(string source)
        {
            return Encoding.ASCII.GetBytes(source);
        }

        public string Decode(byte[] data)
        {
            return Encoding.ASCII.GetString(data);
        }
    }
}
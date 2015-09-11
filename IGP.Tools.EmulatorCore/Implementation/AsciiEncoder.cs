namespace IGP.Tools.EmulatorCore.Implementation
{
    using System.Text;

    internal sealed class AsciiEncoder : IEncoder
    {
        public byte[] Encode(string source) => Encoding.ASCII.GetBytes(source);

        public string Decode(byte[] data) => Encoding.ASCII.GetString(data);
    }
}
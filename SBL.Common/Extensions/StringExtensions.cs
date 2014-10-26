namespace SBL.Common.Extensions
{
    using System.IO;

    public static class StringExtensions
    {
        public static Stream ToStream(this string str)
        {
            Contract.ArgumentIsNotNull(str, () => str);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
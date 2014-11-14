namespace SBL.Common.Extensions
{
    using System.IO;
    using SBL.Common.Annotations;

    public static class Extensions
    {
        [NotNull]
        public static Stream ToStream([NotNull] this string str)
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
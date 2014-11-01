namespace IGP.Tools.IO
{
    using System.Collections.Generic;
    using System.Linq;
    using SBL.Common;
    using SBL.Common.Annotations;

    public static class WellKnownPortTypes
    {
        public const string ConsolePort = "CONSOLE";
        public const string FilePort = "FILE";

        public const string SerialPort = "COM";
        public const string TcpClientPort = "TCP";
        public const string TcpServerPort = "TCPIN";
        public const string UdpPort = "UDP";

        public static readonly IEnumerable<string> Types = 
            new[] { ConsolePort, FilePort, SerialPort, TcpClientPort, TcpServerPort, UdpPort};
        
        internal static bool IsPortTypeWellKnown([NotNull] string portType)
        {
            Contract.ArgumentIsNotNull(portType, () => portType);

            return Types.Contains(portType);
        }
    }
}
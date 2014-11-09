namespace IGP.Tools.IO
{
    public static class PortStates
    {
        public static readonly PortState Connecting = new PortState(
            name: "connecting",
            description: "Port is connecting to the end point",
            isError: false,
            canTransmit: false);

        public static readonly PortState UnableToConnect = new PortState(
            name: "unable to connect",
            description: "unable to connect to the endpoint - connecting failed",
            isError: true,
            canTransmit: false);

        public static readonly PortState Connected = new PortState(
            name: "connected",
            description: "Port is connected to the end point",
            isError: false,
            canTransmit: true);

        public static readonly PortState ConnectionLost = new PortState(
            name: "connection lost",
            description: "Connection to the end point lost",
            isError: true,
            canTransmit: false);

        public static readonly PortState UnknownErrorOccur = new PortState(
            name: "unknown error",
            description: "Unknown error occur",
            isError: true,
            canTransmit: false);

        public static readonly PortState Disconnecting = new PortState(
            name: "disconnecting",
            description: "Port is disconnecting from the end point",
            isError: false,
            canTransmit: false);

        public static readonly PortState Disconnected = new PortState(
            name: "disconnected",
            description: "Port is disconnected from the end point",
            isError: false,
            canTransmit: false);
    }
}
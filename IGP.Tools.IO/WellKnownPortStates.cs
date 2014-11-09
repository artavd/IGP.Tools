namespace IGP.Tools.IO
{
    public static class WellKnownPortStates
    {
        public static readonly PortState Connected = new PortState(
            name: "connected",
            description: "Port is connected to end point",
            isError: false,
            canTransmit: true);

        public static readonly PortState Connecting = new PortState(
            name: "connecting",
            description: "Port is connecting to end point",
            isError: false,
            canTransmit: false);

        public static readonly PortState ConnectionLost = new PortState(
            name: "connection lost",
            description: "Connection to end point is lost",
            isError: true,
            canTransmit: false);

        public static readonly PortState Disconnected = new PortState(
            name: "disconnected",
            description: "Port is disconnected from end point",
            isError: false,
            canTransmit: false);
    }
}
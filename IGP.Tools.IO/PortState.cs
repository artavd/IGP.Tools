namespace IGP.Tools.IO
{
    using System;
    using SBL.Common;
    using SBL.Common.Annotations;

    public struct PortState
    {
        public PortState(
            [NotNull] string name,
            [NotNull] string description = "",
            bool isError = false,
            bool canTransmit = false,
            [CanBeNull] object data = null) : this()
        {
            Contract.ArgumentIsNotNull(name, () => name);
            Contract.ArgumentIsNotNull(description, () => description);

            Name = name;
            Description = description;
            IsError = isError;
            CanTransmit = canTransmit;
            Data = data;
        }

        public string Name { get; }
        public string Description { get; }
        public bool IsError { get; }
        public bool CanTransmit { get; }

        public object Data { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is PortState && Equals((PortState)obj);
        }

        public override string ToString() => Name;
        public override int GetHashCode() => Name.GetHashCode();

        public bool Equals(PortState state) => Name.Equals(state.Name, StringComparison.InvariantCultureIgnoreCase);

        public static bool operator ==(PortState state1, PortState state2) => state1.Equals(state2);
        public static bool operator !=(PortState state1, PortState state2) => !state1.Equals(state2);
    }

    public static class PortStateExtensions
    {
        public static PortState WithData(this PortState state, object data)
        {
            return new PortState(
                name: state.Name,
                description: state.Description,
                isError: state.IsError,
                canTransmit: state.CanTransmit,
                data: data);
        }

        public static PortState WithDescription(this PortState state, string description)
        {
            return new PortState(
                name: state.Name,
                description: description,
                isError: state.IsError,
                canTransmit: state.CanTransmit,
                data: state.Data);
        }
    }
}
namespace IGP.Tools.IO
{
    using System;
    using SBL.Common;
    using SBL.Common.Annotations;

    public struct PortState
    {
        public readonly string Name;
        public readonly string Description;
        public readonly bool IsError;
        public readonly bool CanTransmit;

        public readonly object Data;

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

        public bool Equals(PortState state)
        {
            return Name.Equals(state.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is PortState && Equals((PortState)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(PortState state1, PortState state2)
        {
            return state1.Equals(state2);
        }

        public static bool operator !=(PortState state1, PortState state2)
        {
            return !state1.Equals(state2);
        }
    }
}
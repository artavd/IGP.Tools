namespace IGP.Tools.EmulatorCore.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    public sealed class CyclicValueProvider : IValueProvider
    {
        private readonly List<string> _values = new List<string>();
        private int _nextValueIndex = 0;

        public string Name { get; }

        public CyclicValueProvider([NotNull] string name)
        {
            Name = name;
        }

        public void AddValue([NotNull] string value)
        {
            Contract.ArgumentSatisfied(value, () => value, v => !string.IsNullOrWhiteSpace(v));

            _values.Add(value);
        }

        public void AddValueRange([NotNull] IEnumerable<string> values)
        {
            Contract.ArgumentIsNotNull(values, () => values);

            values.Foreach(AddValue);
        }

        public string GetNextValue()
        {
            if (_values.Count == 0)
            {
                throw new InvalidOperationException("Unable to get next value from empty value set");
            }

            string result = _values.ElementAt(_nextValueIndex);

            _nextValueIndex = (_values.Count == ++_nextValueIndex) ? 0 : _nextValueIndex;

            return result;
        }
    }
}

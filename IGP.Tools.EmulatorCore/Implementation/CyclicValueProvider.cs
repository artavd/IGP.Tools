namespace IGP.Tools.EmulatorCore.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    internal sealed class CyclicValueProvider : IValueProvider
    {
        public string Name { get; private set; }

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
                throw new InvalidOperationException("Unnable to get next value from empty value set");
            }

            string result = _values.ElementAt(_nextValueIndex);

            _nextValueIndex = (_values.Count == ++_nextValueIndex) ? 0 : _nextValueIndex;

            return result;
        }

        private readonly List<string> _values = new List<string>();
        private int _nextValueIndex = 0;
    }
}

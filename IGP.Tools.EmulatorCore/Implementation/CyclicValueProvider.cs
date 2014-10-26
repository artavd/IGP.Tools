namespace IGP.Tools.EmulatorCore.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IGP.Tools.EmulatorCore.Contracts;
    using Microsoft.Practices.ObjectBuilder2;
    using SBL.Common;

    internal sealed class CyclicValueProvider : IValueProvider
    {
        public string Name { get; set; }

        public CyclicValueProvider(string name)
        {
            Name = name;
        }

        public void AddValue(string value)
        {
            Contract.ArgumentIsNotNull(value, () => value);

            _values.Add(value);
        }

        public void AddValueRange(IEnumerable<string> values)
        {
            Contract.ArgumentIsNotNull(values, () => values);

            values.ForEach(AddValue);
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

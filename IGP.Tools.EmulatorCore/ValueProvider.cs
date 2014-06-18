namespace IGP.Tools.EmulatorCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Seterlund.CodeGuard;

    internal sealed class ValueProvider
    {
        public string Name { get; set; }

        public ValueProvider(string name)
        {
            Name = name;
        }

        public void AddValue(string value)
        {
            Guard.That(() => value).IsNotNullOrWhiteSpace();
            
            _values.Add(value);
        }

        public void AddValueRange(IEnumerable<string> values)
        {
            values.ToList().ForEach(AddValue);
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

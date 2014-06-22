namespace IGP.Tools.EmulatorCore
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Microsoft.Practices.ObjectBuilder2;

    using Seterlund.CodeGuard.Internals;

    internal sealed class MessageProvider
    {
        public string Name { get; set; }

        public string FormatString
        {
            get { return _formatString; }
            set { _formatString = ProceedFormatString(value); }
        }

        public IList<IValueProvider> Values { get; private set; }

        public MessageProvider(string formatString)
        {
            FormatString = formatString;
        }

        public string GetNextMessage()
        {
            var valuesArray = new object[Values.Count];
            for (int i = 0; i < Values.Count; i++)
            {
                valuesArray[i] = Values[i].GetNextValue();
            }

            return string.Format(FormatString, valuesArray);
        }

        private string ProceedFormatString(string formatString)
        {
            if (formatString.IsNullOrWhiteSpace())
            {
                throw new IncorrectFormatStringException(
                    "Format string must be non-empty.");
            }

            // Verify all position of value placeholders (in format "{[index]}")
            const string patternCorrectFormat = @"^([^{}]+|\{\d+\})*$";
            if (!Regex.IsMatch(formatString, patternCorrectFormat))
            {
                throw new IncorrectFormatStringException(
                    "Incorrect using of special symbols '{' and '}' in a format string");
            }

            // Find all of value placeholders
            const string patternValue = @"{(\d+)}";
            var matches = Regex.Matches(formatString, patternValue);

            // Mark existance placeholder indexes
            var checkContainsAllNumbers = new bool[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                uint n = uint.Parse(matches[i].Groups[1].Value);
                if (n >= matches.Count)
                {
                    throw new IncorrectFormatStringException(
                        "Placeholder index in format string out of range: {" +
                        n + "}, number " + i);
                }
                checkContainsAllNumbers[n] = true;
            }

            // Check for "holes" in the index numbering (to avoid the situation of "{0} {1} {3}")
            int countFormatIndexes = 0;
            for (int i = 0; i < checkContainsAllNumbers.Length; i++)
            {
                if (i > 0 && checkContainsAllNumbers[i] == true &&
                    checkContainsAllNumbers[i - 1] == false)
                {
                    throw new IncorrectFormatStringException(
                        "\"Hole\" in the index numbering: the index {" + i + "} is exist" +
                        "but the index {" + (i - 1) + "} is not");
                }

                if (checkContainsAllNumbers[i] == true)
                {
                    countFormatIndexes++;
                }
            }

            RecreateValueProviderArray(countFormatIndexes);

            return formatString;
        }

        private void RecreateValueProviderArray(int length)
        {
            Values = new IValueProvider[length];
            Enumerable.Range(0, length).ForEach(i => Values[i] = new VoidValueProvider());
        }

        private string _formatString;
    }
}

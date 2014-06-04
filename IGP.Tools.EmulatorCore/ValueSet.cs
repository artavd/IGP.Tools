namespace IGP.Tools.EmulatorCore
{
    internal sealed class ValueSet
    {
        public string Name { get; set; }

        public string ValueFormat { get; set; }

        public string UserValue { get; set; }

        public GeneratorConfiguration GeneratorConfig { get; private set; }

        public ValueSet(string name)
        {
            Name = name;
            ValueFormat = "F2";
            UserValue = string.Empty;

            GeneratorConfig = default(GeneratorConfiguration);
        }
    }

    internal struct GeneratorConfiguration
    {
        public readonly bool IsEnable;
        public readonly double LowerBound;
        public readonly double UpperBound;

        public GeneratorConfiguration(bool isEnable = false, double lower = 0, double upper = 1000)
        {
            IsEnable = isEnable;
            LowerBound = lower;
            UpperBound = upper;
        }
    }
}

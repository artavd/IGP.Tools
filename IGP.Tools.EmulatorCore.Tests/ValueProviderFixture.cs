namespace IGP.Tools.EmulatorCore.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IGP.Tools.EmulatorCore.Implementation;
    using NUnit.Framework;

    [TestFixture]
    internal sealed class ValueProviderFixture
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetNextValueShouldThrowExceptionIfValuesAreNotAdded()
        {
            // Given
            var provider = new CyclicValueProvider("Provider for test");
            
            // When
            provider.GetNextValue();
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        [ExpectedException(typeof(ArgumentException))]
        public void AddEmptyValueInProviderShouldThrowException(string value)
        {
            // Given
            var provider = new CyclicValueProvider("Provider for test");

            // When
            provider.AddValue(value);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        [ExpectedException(typeof(ArgumentException))]
        public void AddValueRangeWithEmptyValueInProviderShouldThrowException(string value)
        {
            // Given
            var provider = new CyclicValueProvider("Provider for test");
            var range = new[] { "1", "2", value, "4" };

            // When
            provider.AddValueRange(range);
        }

        [Test]
        public void GetNextValueShouldReturnCorrectValueIfItIsOnlyOne()
        {
            // Given
            var provider = new CyclicValueProvider("Provider for test");
            var value = "10";
            provider.AddValue(value);
            
            // When
            int n = 5;
            var results = new string[n];
            for (int i = 0; i < n; i++)
            {
                results[i] = provider.GetNextValue();
            }

            // Then
            Assert.That(results, Is.All.EqualTo(value));
        }

        [Test]
        public void GetNextValueShouldReturnLoopSequenceOfContainedValues()
        {
            // Given
            var provider = new CyclicValueProvider("Provider for test");
            var values = new[] { "1", "2", "3", "4" };
            provider.AddValueRange(values);

            // When
            int n = 3;
            var results = new string[values.Length * n];
            for (int i = 0; i < values.Length * n; i++)
            {
                results[i] = provider.GetNextValue();
            }

            // Then
            var expectedResults = Enumerable.Repeat(values, 3).SelectMany(arr => arr as IEnumerable<string>);
            CollectionAssert.AreEqual(results, expectedResults);
        }
    }
}

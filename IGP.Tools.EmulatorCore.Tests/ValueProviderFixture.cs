namespace IGP.Tools.EmulatorCore.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    internal sealed class ValueProviderFixture
    {
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetNextValueShouldThrowExceptionIfValuesAreNotAdded()
        {
            // Given
            var provider = new ValueProvider("Provider for test");
            
            // When
            provider.GetNextValue();
        }

        [Test]
        public void GetNextValueShouldReturnCorrectValueIfItIsOnlyOne()
        {
            // Given
            var provider = new ValueProvider("Provider for test");
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
            var provider = new ValueProvider("Provider for test");
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

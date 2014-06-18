namespace IGP.Tools.EmulatorCore.Tests
{
    using NUnit.Framework;

    [TestFixture]
    internal sealed class MessageProviderFixture
    {
        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        [TestCase("incorrect {} format string")]
        [TestCase("incorrect {0}} format string")]
        [TestCase("incorrect format { string")]
        [TestCase("incorrect format } string")]
        [TestCase("incorrect format {1 string")]
        [TestCase("incorrect format 1} string")]
        [TestCase("incorrect {0} format {0} {2} string")]
        [TestCase("incorrect {1} format string")]
        [ExpectedException(typeof(IncorrectFormatStringException))]
        public void CreatingMessageProviderWithIncorrectFormatStringShouldThrowException(string formatString)
        {
            // Given
            var message = new MessageProvider(formatString);
        }

        [TestCase("correct format string", 0)]
        [TestCase("correct {0} format string", 1)]
        [TestCase("correct {0} format {1} string", 2)]
        [TestCase("correct {1}{0} format string", 2)]
        [TestCase("correct {0} format {0} string", 1)]
        [TestCase("{2} correct {0} {1} format {1}{0}{2}string", 3)]
        public void CreatingMessageProviderWithCorrectFormatStringShouldCreateCorrectValuesList(string formatString, int valueCount)
        {
            // Given
            var message = new MessageProvider(formatString);

            // Then
            Assert.AreEqual(valueCount, message.Values.Count);
        }

        // TODO: make mock for ValueProvider
        [Ignore]
        public void GetNextMessageShouldWorkCorrectly()
        {
            // Given
            var message = new MessageProvider("format {0} string {1}");

            // Then
        }

        [Test]
        public void MessageProviderWithoutValuesShouldWorkCorrectly()
        {
            // Given
            const string formatString = "Format string without any value placeholders";
            var message = new MessageProvider(formatString);

            // Then
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(formatString, message.GetNextMessage());
            }
        }
    }
}
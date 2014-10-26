namespace IGP.Tools.EmulatorCore.Tests
{
    using IGP.Tools.EmulatorCore.Contracts;
    using IGP.Tools.EmulatorCore.Implementation;
    using Moq;

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
            Assert.AreEqual(valueCount, message.Values.Length);
        }

        [Test]
        public void GetNextMessageForNonInitializedMessageProviderShouldReturnNullPlaceholders()
        {
            // Given
            const string format = "format {0} string {1}";
            var nullValue = new VoidValueProvider().GetNextValue();
            var message = new MessageProvider(format);
            
            // When
            var generatedMessage = message.GetNextMessage();

            // Then
            Assert.AreEqual(string.Format(format, nullValue, nullValue), generatedMessage);
        }

        [TestCase(1, "format {0} string")]
        [TestCase(1, "format {0} string {1}")]
        [TestCase(1, "format {0} string {1} {2}")]
        [TestCase(2, "format {0} string")]
        [TestCase(2, "format {0} string {1}")]
        [TestCase(2, "format {0} string {1} {2}")]
        [TestCase(3, "format {0} string")]
        [TestCase(3, "format {0} string {1}")]
        [TestCase(3, "format {0} string {1} {2}")]
        public void GetNextMessageForInitializedMessageProviderShouldCallGetNextValueForAllValueProviderForEveryCall(int callCount, string format)
        {
            // Given
            var message = new MessageProvider(format);

            var mocks = new Mock<IValueProvider>[message.Values.Length];
            for (int i = 0; i < message.Values.Length; i++)
            {
                mocks[i] = new Mock<IValueProvider>();
                mocks[i].Setup(vp => vp.GetNextValue()).Returns("<value>");
                message.Values[i] = mocks[i].Object;
            }

            // When
            for (int i = 0; i < callCount; i++)
            {
                message.GetNextMessage();
            }

            // Then
            foreach (var mock in mocks)
            {
                mock.Verify(vp => vp.GetNextValue(), Times.Exactly(callCount));
            }
        }

        [TestCase("format {0} string", new [] { "1" })]
        [TestCase("format {0} string {1} {2}", new [] { "1", "2", "3" })]
        public void GetNextMessageShouldReturnCorrectMessageString(string format, string[] values)
        {
            // Given
            var message = new MessageProvider(format);

            var mocks = new Mock<IValueProvider>[message.Values.Length];
            for (int i = 0; i < message.Values.Length; i++)
            {
                mocks[i] = new Mock<IValueProvider>();
                mocks[i].Setup(vp => vp.GetNextValue()).Returns(values[i]);
                message.Values[i] = mocks[i].Object;
            }

            // When
            var result = message.GetNextMessage();

            // Then
            Assert.AreEqual(string.Format(format, values), result);
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
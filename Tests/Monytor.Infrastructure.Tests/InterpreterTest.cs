using FluentAssertions;
using Monytor.Infrastructure.Helper;
using System;
using Xunit;

namespace Monytor.Infrastructure.Tests {
    public class InterpreterTest {
        [Fact]
        public void ReplacePlaceholder_NoPlaceholdersAvailable_ReturnsOrginalString() {
            // Arrange
            var interpreter = new Interpreter();
            var text = "This is a text without placeholders";

            // Act
            var result = interpreter.ReplacePlaceholder(text);

            // Arrange
            result.Should().Be(text);
        }

        [Fact]
        public void ReplacePlaceholder_EmptyString_ReturnsOrginalString() {
            // Arrange
            var interpreter = new Interpreter();
            var text = "";

            // Act
            var result = interpreter.ReplacePlaceholder(text);

            // Arrange
            result.Should().Be(text);
        }

        [Fact]
        public void ReplacePlaceholder_Null_ReturnsNull() {
            // Arrange
            var interpreter = new Interpreter();

            // Act
            var result = interpreter.ReplacePlaceholder(null);

            // Arrange
            result.Should().BeNull();
        }

        [Fact]
        public void ReplacePlaceholder_WithPlaceholderDateTimeUtcNow_ReturnsReplacement() {
            // Arrange
            var interpreter = new Interpreter();
            var text = "The current {{DATETIME.UTCNOW}}";
            var expectedResult = $"The current {DateTime.UtcNow.ToString("o")}";
            expectedResult = expectedResult.Substring(0, expectedResult.Length - 13);
            
            // Act
            var result = interpreter.ReplacePlaceholder(text);

            // Arrange
            result.Should().StartWith(expectedResult);
            result.Should().NotContain("{{");
            result.Should().NotContain("}}");
        }

        [Fact]
        public void ReplacePlaceholder_WithPlaceholderDateTimeUtcNowMinusDays_ReturnsReplacement() {
            // Arrange
            var interpreter = new Interpreter();
            var text = "The current {{DATETIME.UTCNOW-14.00:00:00}}";
            var dateTime = DateTime.UtcNow - TimeSpan.Parse("14.00:00:00");
            var expectedResult = $"The current {dateTime.ToString("o")}";
            expectedResult = expectedResult.Substring(0, expectedResult.Length - 13);

            // Act
            var result = interpreter.ReplacePlaceholder(text);

            // Arrange
            result.Should().StartWith(expectedResult);
            result.Should().NotContain("{{");
            result.Should().NotContain("}}");
        }

        [Fact]
        public void ReplacePlaceholder_WithPlaceholderDateTimeUtcNowMinusHours_ReturnsReplacement() {
            // Arrange
            var interpreter = new Interpreter();
            var text = "The current {{DATETIME.UTCNOW-06:00:00}}";
            var dateTime = DateTime.UtcNow - TimeSpan.Parse("06:00:00");
            var expectedResult = $"The current {dateTime.ToString("o")}";
            expectedResult = expectedResult.Substring(0, expectedResult.Length - 13);

            // Act
            var result = interpreter.ReplacePlaceholder(text);

            // Arrange
            result.Should().StartWith(expectedResult);
            result.Should().NotContain("{{");
            result.Should().NotContain("}}");
        }

        [Fact]
        public void ReplacePlaceholder_WithPlaceholderDateTimeUtcNowPlusDays_ReturnsReplacement() {
            // Arrange
            var interpreter = new Interpreter();
            var text = "The current {{DATETIME.UTCNOW+14.00:00:00}}";
            var dateTime = DateTime.UtcNow + TimeSpan.Parse("14.00:00:00");
            var expectedResult = $"The current {dateTime.ToString("o")}";
            expectedResult = expectedResult.Substring(0, expectedResult.Length - 13);

            // Act
            var result = interpreter.ReplacePlaceholder(text);

            // Arrange
            result.Should().StartWith(expectedResult);
            result.Should().NotContain("{{");
            result.Should().NotContain("}}");
        }

        [Fact]
        public void ReplacePlaceholder_WithPlaceholderDateTimeUtcNowPlusHours_ReturnsReplacement() {
            // Arrange
            var interpreter = new Interpreter();
            var text = "The current {{DATETIME.UTCNOW+06:00:00}}";
            var dateTime = DateTime.UtcNow + TimeSpan.Parse("06:00:00");
            var expectedResult = $"The current {dateTime.ToString("o")}";
            expectedResult = expectedResult.Substring(0, expectedResult.Length - 13);

            // Act
            var result = interpreter.ReplacePlaceholder(text);

            // Arrange
            result.Should().StartWith(expectedResult);
            result.Should().NotContain("{{");
            result.Should().NotContain("}}");
        }

        [Fact]
        public void ReplacePlaceholder_WithPlaceholderDateTimeUtcNowAndDateTimeUtcNowPlus_ReturnsReplacement() {
            // Arrange
            var interpreter = new Interpreter();
            var text = "The current {{DATETIME.UTCNOW}} plus {{DATETIME.UTCNOW+7.00:00:00}} end";
            var dateTime = DateTime.UtcNow;
            var expectedResult = $"The current {dateTime.ToString("o")}";
            expectedResult = expectedResult.Substring(0, expectedResult.Length - 13);

            // Act
            var result = interpreter.ReplacePlaceholder(text);

            // Arrange
            result.Should().StartWith(expectedResult);
            result.Should().NotContain("{{");
            result.Should().NotContain("}}");
            result.EndsWith("end");
        }

        [Fact]
        public void ReplacePlaceholder_WithPlaceholderDateTimeUtcNowAndDateTimeUtcNowPlusAndDateTimeUtcNowMinus_ReturnsReplacement() {
            // Arrange
            var interpreter = new Interpreter();
            var text = "The current {{DATETIME.UTCNOW}} plus {{DATETIME.UTCNOW+7.00:00:00}} minus {{DATETIME.UTCNOW-7.00:00:00}} end";
            var dateTime = DateTime.UtcNow;
            var expectedResult = $"The current {dateTime.ToString("o")}";
            expectedResult = expectedResult.Substring(0, expectedResult.Length - 13);

            // Act
            var result = interpreter.ReplacePlaceholder(text);

            // Arrange
            result.Should().StartWith(expectedResult);
            result.Should().NotContain("{{");
            result.Should().NotContain("}}");
            result.EndsWith("end");
        }
    }
}

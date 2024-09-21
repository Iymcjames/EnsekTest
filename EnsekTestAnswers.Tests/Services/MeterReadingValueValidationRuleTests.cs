using EnsekTestAnswers.Dtos;
using EnsekTestAnswers.Models;
using EnsekTestAnswers.Services;

namespace EnsekTestAnswers.Tests.Services
{


    public class MeterReadingValueValidationRuleTests
    {
        private readonly MeterReadingValueValidationRule _validationRule;

        public MeterReadingValueValidationRuleTests()
        {
            _validationRule = new MeterReadingValueValidationRule();
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturnNoErrors_WhenMeterReadValueIsValid()
        {
            // Arrange
            var reading = new MeterReadingDto
            {
                AccountId = "1",
                MeterReadValue = "50000",
                MeterReadingDateTime = "01/09/2023 10:00"
            };

            var validReadings = new List<MeterReading>();

            // Act
            var result = await _validationRule.ValidateAsync(reading, validReadings);

            // Assert
            Assert.Empty(result.ToString());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturnError_WhenMeterReadValueIsNegative()
        {
            // Arrange
            var reading = new MeterReadingDto
            {
                AccountId = "1",
                MeterReadValue = "-10",
                MeterReadingDateTime = "01/09/2023 10:00"
            };

            var validReadings = new List<MeterReading>();

            // Act
            var result = await _validationRule.ValidateAsync(reading, validReadings);

            // Assert
            Assert.Contains("Invalid Meter Reading value for Account 1: meter reading value -10", result.ToString());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturnError_WhenMeterReadValueIsAboveMax()
        {
            // Arrange
            var reading = new MeterReadingDto
            {
                AccountId = "1",
                MeterReadValue = "100000",
                MeterReadingDateTime = "01/09/2023 10:00"
            };

            var validReadings = new List<MeterReading>();

            // Act
            var result = await _validationRule.ValidateAsync(reading, validReadings);

            // Assert
            Assert.Contains("Invalid Meter Reading value for Account 1: meter reading value 100000", result.ToString());
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturnError_WhenMeterReadValueIsNotANumber()
        {
            // Arrange
            var reading = new MeterReadingDto
            {
                AccountId = "1",
                MeterReadValue = "not-a-number",
                MeterReadingDateTime = "01/09/2023 10:00"
            };

            var validReadings = new List<MeterReading>();

            // Act
            var result = await _validationRule.ValidateAsync(reading, validReadings);

            // Assert
            Assert.Contains("Invalid Meter Reading value for Account 1: meter reading value not-a-number", result.ToString());
        }
    }

}

using EnsekTestAnswers.Dtos;
using EnsekTestAnswers.Models;
using EnsekTestAnswers.Services;


namespace EnsekTestAnswers.Tests.Services;


public class DuplicateReadingValidationRuleTests
{
    private readonly DuplicateReadingValidationRule _rule;

    public DuplicateReadingValidationRuleTests()
    {
        _rule = new DuplicateReadingValidationRule();
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenDuplicateReadingExists()
    {
        // Arrange
        var reading = new MeterReadingDto
        {
            AccountId = "1",
            MeterReadingDateTime = "01/09/2023 10:00",
            MeterReadValue = "100"
        };

        var validReadings = new List<MeterReading>
    {
        new MeterReading
        {
            AccountId = 1,
            MeterReadingDateTime = new DateTime(2023, 9, 1, 10, 0, 0),
            MeterReadValue = 100
        }
    };

        // Act
        var result = await _rule.ValidateAsync(reading, validReadings);

        // Assert
        Assert.NotEmpty(result.ToString());
        Assert.Contains("Duplicate meter reading for Account 1 at 01/09/2023 10:00:00", result.ToString());
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnNoError_WhenNoDuplicateReadingExists()
    {
        // Arrange
        var reading = new MeterReadingDto
        {
            AccountId = "1",
            MeterReadingDateTime = "01/09/2023 10:00",
            MeterReadValue = "100"
        };

        var validReadings = new List<MeterReading>
        {
            new MeterReading
            {
                AccountId = 1,
                MeterReadingDateTime = new DateTime(2023, 9, 1, 9, 0, 0),
                MeterReadValue = 99
            }
        };

        // Act
        var result = await _rule.ValidateAsync(reading, validReadings);

        // Assert
        Assert.Empty(result.ToString());
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnNoError_WhenAccountIdIsInvalid()
    {
        // Arrange
        var reading = new MeterReadingDto
        {
            AccountId = "invalid",
            MeterReadingDateTime = "01/09/2023 10:00",
            MeterReadValue = "100"
        };

        var validReadings = new List<MeterReading>();

        // Act
        var result = await _rule.ValidateAsync(reading, validReadings);

        // Assert
        Assert.Empty(result.ToString());
    }
}

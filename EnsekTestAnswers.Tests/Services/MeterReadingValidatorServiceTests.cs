using EnsekTestAnswers.Dtos;
using EnsekTestAnswers.Models;
using EnsekTestAnswers.Services;
using Moq;
using System.Text;

namespace EnsekTestAnswers.Tests.Services;

public class MeterReadingValidatorServiceTests
{
    private readonly MeterReadingValidatorService _validatorService;
    private readonly Mock<IMeterReadingValidationRule> _validationRuleMock;

    public MeterReadingValidatorServiceTests()
    {
        _validationRuleMock = new Mock<IMeterReadingValidationRule>();
        var validationRules = new List<IMeterReadingValidationRule> { _validationRuleMock.Object };
        _validatorService = new MeterReadingValidatorService(validationRules);
    }

    [Fact]
    public async Task ValidateMeterReadingsAsync_ShouldReturnNoErrors_AndAddValidReading()
    {
        // Arrange
        var readings = new List<MeterReadingDto>
        {
            new MeterReadingDto { AccountId = "1", MeterReadingDateTime = "01/09/2023 10:00", MeterReadValue = "100" }
        };

        var validReadings = new List<MeterReading>();

        _validationRuleMock.Setup(v => v.ValidateAsync(It.IsAny<MeterReadingDto>(), It.IsAny<IList<MeterReading>>()))
            .ReturnsAsync(new StringBuilder());

        // Act
        var result = await _validatorService.ValidateMeterReadingsAsync(readings, validReadings);

        // Assert
        Assert.Empty(result.ToString());
        Assert.Single(validReadings);
        Assert.Equal(1, validReadings[0].AccountId);
        Assert.Equal(new DateTime(2023, 9, 1, 10, 0, 0), validReadings[0].MeterReadingDateTime);
        Assert.Equal(100, validReadings[0].MeterReadValue);
    }

    [Fact]
    public async Task ValidateMeterReadingsAsync_ShouldReturnValidationErrors_AndNotAddInvalidReading()
    {
        // Arrange
        var readings = new List<MeterReadingDto>
        {
            new MeterReadingDto { AccountId = "1", MeterReadingDateTime = "invalid", MeterReadValue = "100" }
        };

        var validReadings = new List<MeterReading>();

        var validationError = new StringBuilder("Invalid date format.");
        _validationRuleMock.Setup(v => v.ValidateAsync(It.IsAny<MeterReadingDto>(), It.IsAny<IList<MeterReading>>()))
            .ReturnsAsync(validationError);

        // Act
        var result = await _validatorService.ValidateMeterReadingsAsync(readings, validReadings);

        // Assert
        Assert.Equal("Invalid date format.", result.ToString());
        Assert.Empty(validReadings);
    }

    [Fact]
    public async Task ValidateMeterReadingsAsync_ShouldReturnNoErrors_WhenNoReadingsProvided()
    {
        // Arrange
        var readings = new List<MeterReadingDto>();
        var validReadings = new List<MeterReading>();

        // Act
        var result = await _validatorService.ValidateMeterReadingsAsync(readings, validReadings);

        // Assert
        Assert.Empty(result.ToString());
        Assert.Empty(validReadings);
    }

    [Fact]
    public async Task ValidateMeterReadingsAsync_ShouldAccumulateErrors_ForMultipleReadings()
    {
        // Arrange
        var readings = new List<MeterReadingDto>
        {
            new MeterReadingDto { AccountId = "1", MeterReadingDateTime = "invalid", MeterReadValue = "100" },
            new MeterReadingDto { AccountId = "2", MeterReadingDateTime = "02/09/2023 10:00", MeterReadValue = "200" }
        };

        var validReadings = new List<MeterReading>();

        var validationError1 = new StringBuilder("Invalid date format.");
        _validationRuleMock.Setup(v => v.ValidateAsync(readings[0], It.IsAny<IList<MeterReading>>()))
            .ReturnsAsync(validationError1);

        var validationError2 = new StringBuilder();
        _validationRuleMock.Setup(v => v.ValidateAsync(readings[1], It.IsAny<IList<MeterReading>>()))
            .ReturnsAsync(validationError2);

        // Act
        var result = await _validatorService.ValidateMeterReadingsAsync(readings, validReadings);

        // Assert
        Assert.Equal("Invalid date format.", result.ToString());
        Assert.Single(validReadings);
        Assert.Equal(2, validReadings[0].AccountId);
        Assert.Equal(new DateTime(2023, 9, 2, 10, 0, 0), validReadings[0].MeterReadingDateTime);
        Assert.Equal(200, validReadings[0].MeterReadValue);
    }
}


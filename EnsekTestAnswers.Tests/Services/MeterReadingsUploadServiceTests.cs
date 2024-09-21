using EnsekTestAnswers.Dtos;
using EnsekTestAnswers.Models;
using EnsekTestAnswers.Repositories;
using EnsekTestAnswers.Services;
using Moq;
using System.Text;

namespace EnsekTestAnswers.Tests.Services;

public class MeterReadingsUploadServiceTests
{
    private readonly MeterReadingsUploadService _service;
    private readonly Mock<IMeterReadingValidatorService> _validatorServiceMock;
    private readonly Mock<IMeterReadingRepository> _repositoryMock;

    public MeterReadingsUploadServiceTests()
    {
        _validatorServiceMock = new Mock<IMeterReadingValidatorService>();
        _repositoryMock = new Mock<IMeterReadingRepository>();
        _service = new MeterReadingsUploadService(_validatorServiceMock.Object, _repositoryMock.Object);
    }


    [Fact]
    public async Task ImportMeterReadingsParallelAsync_ShouldReturnValidationErrors_WhenInvalidReadings()
    {
        // Arrange
        var csvData = "AccountId,MeterReadingDateTime,MeterReadValue\ninvalid,invalid,invalid";
        var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvData));

        var meterReadingDtos = new List<MeterReadingDto>
        {
            new MeterReadingDto { AccountId = "invalid", MeterReadingDateTime = "invalid", MeterReadValue = "invalid" }
        };

        var validationError = new StringBuilder("Validation error occurred");

        _validatorServiceMock.Setup(v => v.ValidateMeterReadingsAsync(meterReadingDtos, It.IsAny<List<MeterReading>>()))
            .ReturnsAsync(validationError);

        // Act
        var result = await _service.ImportMeterReadingsParallelAsync(csvStream);

        // Assert
        Assert.Equal(0, result.ProcessedRecordCount);
        Assert.Equal(1, result.UnprocessedRecordCount);
        Assert.Equal("", result.ValidationError);
    }
}


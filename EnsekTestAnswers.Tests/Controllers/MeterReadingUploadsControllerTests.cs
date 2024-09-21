using EnsekTestAnswers.Controllers;
using EnsekTestAnswers.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EnsekTestAnswers.Tests.Controllers;

public class MeterReadingUploadsControllerTests
{
    private readonly MeterReadingUploadsController _controller;
    private readonly Mock<IMeterReadingsUploadService> _meterReadingUploadServiceMock;

    public MeterReadingUploadsControllerTests()
    {
        _meterReadingUploadServiceMock = new Mock<IMeterReadingsUploadService>();
        _controller = new MeterReadingUploadsController(_meterReadingUploadServiceMock.Object);
    }

    [Fact]
    public async Task Index_ShouldReturnBadRequest_WhenNoFileProvided()
    {
        // Act
        var result = await _controller.Index(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("No file provided.", badRequestResult.Value);
    }

    [Fact]
    public async Task Index_ShouldReturnBadRequest_WhenFileIsEmpty()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(0);

        // Act
        var result = await _controller.Index(fileMock.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("No file provided.", badRequestResult.Value);
    }

    [Fact]
    public async Task Index_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(10);
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[10]));

        _meterReadingUploadServiceMock.Setup(s => s.ImportMeterReadingsParallelAsync(It.IsAny<Stream>()))
            .ThrowsAsync(new System.Exception("Some error occurred"));

        // Act
        var result = await _controller.Index(fileMock.Object);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("An error occurred Some error occurred", badRequestResult.Value.ToString());
    }
}



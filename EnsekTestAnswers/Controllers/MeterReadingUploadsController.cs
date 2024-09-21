using EnsekTestAnswers.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnsekTestAnswers.Controllers;

[ApiController]
[Route("api")]
public class MeterReadingUploadsController : ControllerBase
{
    private readonly IMeterReadingsUploadService _meterReadingUploadService;

    public MeterReadingUploadsController(IMeterReadingsUploadService meterReadingUploadService)
    {
        _meterReadingUploadService = meterReadingUploadService;
    }

    [HttpPost("meter-reading-uploads")]
    public async Task<IActionResult> Index(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            using var stream = file.OpenReadStream();
            var result = await _meterReadingUploadService.ImportMeterReadingsParallelAsync(stream);

            return Ok(new { Message = "Meter readings imported.", result.ProcessedRecordCount, result.UnprocessedRecordCount, result.ValidationError });

        }
        catch (Exception ex)
        {
            return BadRequest($"An error occurred {ex.Message}");
        }
    }
}

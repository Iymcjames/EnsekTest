using EnsekTestAnswers.Dtos;
using EnsekTestAnswers.Models;
using EnsekTestAnswers.Repositories;

namespace EnsekTestAnswers.Services;


public class MeterReadingsUploadService : IMeterReadingsUploadService
{
    private readonly IMeterReadingValidatorService _meterReadingValidatorService;
    private readonly IMeterReadingRepository _meterReadingRepository;

    public MeterReadingsUploadService(IMeterReadingValidatorService meterReadingValidatorService, IMeterReadingRepository meterReadingRepository)
    {
        _meterReadingValidatorService = meterReadingValidatorService;
        _meterReadingRepository = meterReadingRepository;
    }

    public async Task<ImportResult> ImportMeterReadingsParallelAsync(Stream csvStream)
    {
        var meterReadingDtos = await GetMeterReadingsAsync(csvStream);
        var validReadings = new List<MeterReading>();

        var validationErrors = await _meterReadingValidatorService.ValidateMeterReadingsAsync(meterReadingDtos, validReadings);

        if (validReadings.Any())
        {
            await _meterReadingRepository.BulkUploadMeterReadingsAsync(validReadings);
        }

        return new ImportResult
        {
            ValidationError = validationErrors != null ? validationErrors.ToString() : "",
            ProcessedRecordCount = validReadings.Count,
            UnprocessedRecordCount = meterReadingDtos.Count - validReadings.Count
        };
    }

    private async Task<List<MeterReadingDto>> GetMeterReadingsAsync(Stream csvStream)
    {
        var meterReadingDtos = new List<MeterReadingDto>();

        using var reader = new StreamReader(csvStream);

        await reader.ReadLineAsync();

        string line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            var values = line.Split(',');

            if (values.Length >= 3)
            {
                meterReadingDtos.Add(new MeterReadingDto
                {
                    AccountId = values[0],
                    MeterReadingDateTime = values[1],
                    MeterReadValue = values[2]
                });
            }
        }

        return meterReadingDtos;
    }

}

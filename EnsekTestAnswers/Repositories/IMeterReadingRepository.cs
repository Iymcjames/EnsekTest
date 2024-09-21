using EnsekTestAnswers.Models;

namespace EnsekTestAnswers.Repositories;

public interface IMeterReadingRepository
{
    Task BulkUploadMeterReadingsAsync(IEnumerable<MeterReading> meterReadings);
    Task<MeterReading> GetByAccountIdAsync(int accountId);
}

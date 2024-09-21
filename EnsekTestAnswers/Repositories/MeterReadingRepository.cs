using EnsekTestAnswers.Data;
using EnsekTestAnswers.Models;
using Microsoft.EntityFrameworkCore;

namespace EnsekTestAnswers.Repositories;
public class MeterReadingRepository : IMeterReadingRepository
{
    private readonly AppDbContext _context;

    public MeterReadingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task BulkUploadMeterReadingsAsync(IEnumerable<MeterReading> meterReadings)
    {
        if (!meterReadings.Any())
        {
            return;
        }

        await _context.BulkInsertAsync(meterReadings);
    }

    public async Task<MeterReading> GetByAccountIdAsync(int accountId)
    {
        return await _context.MeterReadings.FirstOrDefaultAsync(a => a.AccountId == accountId);
    }
}

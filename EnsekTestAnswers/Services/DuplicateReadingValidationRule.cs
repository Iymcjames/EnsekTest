using System.Globalization;
using System.Text;
using EnsekTestAnswers.Dtos;
using EnsekTestAnswers.Models;

namespace EnsekTestAnswers.Services;

public class DuplicateReadingValidationRule : IMeterReadingValidationRule
{
    public Task<StringBuilder> ValidateAsync(MeterReadingDto reading, IList<MeterReading> validReadings)
    {
        var errors = new StringBuilder();
        if (!int.TryParse(reading.AccountId, out int accountId))
            return Task.FromResult(errors);

        var meterReadingDateTime = ConvertToDateTime(reading.MeterReadingDateTime);
        if (meterReadingDateTime.HasValue)
        {
            var duplicate = validReadings.FirstOrDefault(x => x.AccountId == accountId &&
                                                              x.MeterReadingDateTime == meterReadingDateTime.Value &&
                                                              x.MeterReadValue == int.Parse(reading.MeterReadValue));

            if (duplicate != null)
            {
                errors.AppendLine($"Duplicate meter reading for Account {reading.AccountId} at {meterReadingDateTime}.");
            }
        }

        return Task.FromResult(errors);
    }

    private DateTime? ConvertToDateTime(string? dateStr)
    {
        if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            return parsedDate;
        }

        return null;
    }
}

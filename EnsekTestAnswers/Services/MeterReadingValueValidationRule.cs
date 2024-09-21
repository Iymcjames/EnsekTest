using System.Text;
using EnsekTestAnswers.Dtos;
using EnsekTestAnswers.Models;

namespace EnsekTestAnswers.Services;

public class MeterReadingValueValidationRule : IMeterReadingValidationRule
{
    public Task<StringBuilder> ValidateAsync(MeterReadingDto reading, IList<MeterReading> validReadings)
    {
        var errors = new StringBuilder();

        if (!int.TryParse(reading.MeterReadValue, out int meterReading) || meterReading < 0 || meterReading > 99999)
        {
            errors.AppendLine($"Invalid Meter Reading value for Account {reading.AccountId}: meter reading value {reading.MeterReadValue}");
        }

        return Task.FromResult(errors);
    }
}

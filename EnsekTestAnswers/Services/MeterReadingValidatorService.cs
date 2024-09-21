using System.Globalization;
using System.Text;
using EnsekTestAnswers.Dtos;
using EnsekTestAnswers.Models;

namespace EnsekTestAnswers.Services;

public class MeterReadingValidatorService : IMeterReadingValidatorService
{
    private readonly IEnumerable<IMeterReadingValidationRule> _validationRules;

    public MeterReadingValidatorService(IEnumerable<IMeterReadingValidationRule> validationRules)
    {
        _validationRules = validationRules;
    }

    public async Task<StringBuilder> ValidateMeterReadingsAsync(IEnumerable<MeterReadingDto> readings, IList<MeterReading> validReadings)
    {
        validReadings.Clear();
        var allValidationErrors = new StringBuilder();

        if (!readings.Any())
            return allValidationErrors;

        foreach (var reading in readings)
        {
            var validationError = new StringBuilder();
            foreach (var rule in _validationRules)
            {
                var errors = await rule.ValidateAsync(reading, validReadings);
                validationError.Append(errors);
            }

            if (validationError.Length == 0)
                validReadings.Add(await ConvertToMeterReadingAsync(reading));
            else
                allValidationErrors.Append(validationError);
        }

        return allValidationErrors;
    }

    private Task<MeterReading> ConvertToMeterReadingAsync(MeterReadingDto reading)
    {
        return Task.FromResult(new MeterReading
        {
            AccountId = int.Parse(reading.AccountId),
            MeterReadingDateTime = DateTime.ParseExact(reading.MeterReadingDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
            MeterReadValue = int.Parse(reading.MeterReadValue)
        });
    }
}

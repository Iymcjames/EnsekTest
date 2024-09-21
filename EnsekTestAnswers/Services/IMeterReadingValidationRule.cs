using System.Text;
using EnsekTestAnswers.Dtos;
using EnsekTestAnswers.Models;

namespace EnsekTestAnswers.Services;

public interface IMeterReadingValidationRule
{
    Task<StringBuilder> ValidateAsync(MeterReadingDto reading, IList<MeterReading> validReadings);
}

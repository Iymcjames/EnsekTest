using System.Text;
using EnsekTestAnswers.Dtos;
using EnsekTestAnswers.Models;

namespace EnsekTestAnswers.Services;

public interface IMeterReadingValidatorService
{
    Task<StringBuilder> ValidateMeterReadingsAsync(IEnumerable<MeterReadingDto> readings, IList<MeterReading> validReadings);
}
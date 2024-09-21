using EnsekTestAnswers.Models;

namespace EnsekTestAnswers.Services;

public interface IMeterReadingsUploadService

{
    Task<ImportResult> ImportMeterReadingsParallelAsync(Stream csvStream);
}
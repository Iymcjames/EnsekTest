namespace EnsekTestAnswers.Models;

public class ImportResult
{
    public int ProcessedRecordCount { get; set; }
    public int UnprocessedRecordCount { get; set; }
    public string ValidationError { get; set; }
}

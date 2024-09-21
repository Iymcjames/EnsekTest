using CsvHelper;
using EnsekTestAnswers.Models;
using System.Globalization;

namespace EnsekTestAnswers.Data;

public static class Seed
{
    public static async Task SeedDatabaseAsync(AppDbContext context)
    {
        if (context.Accounts.Any())
        {
            return;
        }

        using (var reader = new StreamReader("Test_Accounts.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<Account>().ToList();
            await context.Accounts.AddRangeAsync(records);
            await context.SaveChangesAsync();
        }

    }
}

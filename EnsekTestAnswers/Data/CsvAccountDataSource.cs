using EnsekTestAnswers.Models;

namespace EnsekTestAnswers.Data;

public class CsvAccountDataSource : IAccountDataSource
{
    public async Task<IEnumerable<Account>> GetAccountDataAsync(string connectionUrl = "Test_Accounts.csv")
    {
        var accounts = new List<Account>();
        var csvLines = await File.ReadAllLinesAsync(connectionUrl);
        foreach (var line in csvLines.Skip(1))
        {
            var values = line.Split(',');
            accounts.Add(new Account
            {
                AccountId = int.Parse(values[0]),
                FirstName = values[1],
                LastName = values[2]
            });
        }
        return accounts;
    }
}
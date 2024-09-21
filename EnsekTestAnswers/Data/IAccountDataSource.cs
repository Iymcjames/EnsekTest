using EnsekTestAnswers.Models;

namespace EnsekTestAnswers.Data;

public interface IAccountDataSource
{
    Task<IEnumerable<Account>> GetAccountDataAsync(string connectionUrl = "Test_Accounts.csv");
}

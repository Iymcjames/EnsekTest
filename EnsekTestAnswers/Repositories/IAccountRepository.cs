using EnsekTestAnswers.Models;

namespace EnsekTestAnswers.Repositories;

public interface IAccountRepository
{
    Task<Account> GetByAccountIdAsync(int accountId);
}

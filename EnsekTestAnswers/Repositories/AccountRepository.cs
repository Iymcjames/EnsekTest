using EnsekTestAnswers.Data;
using EnsekTestAnswers.Models;
using Microsoft.EntityFrameworkCore;

namespace EnsekTestAnswers.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Account> GetByAccountIdAsync(int accountId)
    {
        return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
    }
}


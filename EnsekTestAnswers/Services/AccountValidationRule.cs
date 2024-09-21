using System.Text;
using EnsekTestAnswers.Dtos;
using EnsekTestAnswers.Models;
using EnsekTestAnswers.Repositories;

namespace EnsekTestAnswers.Services;

public class AccountValidationRule : IMeterReadingValidationRule
{
    private readonly IAccountRepository _accountRepository;

    public AccountValidationRule(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<StringBuilder> ValidateAsync(MeterReadingDto reading, IList<MeterReading> validReadings)
    {
        var errors = new StringBuilder();
        if (!int.TryParse(reading.AccountId, out int accountId))
        {
            errors.AppendLine($"Invalid AccountId format: {reading.AccountId}");
            return errors;
        }

        var account = await _accountRepository.GetByAccountIdAsync(accountId);
        if (account == null)
        {
            errors.AppendLine($"Account not found: {accountId}");
        }

        return errors;
    }
}

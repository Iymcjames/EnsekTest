using EnsekTestAnswers.Dtos;
using EnsekTestAnswers.Models;
using EnsekTestAnswers.Repositories;
using EnsekTestAnswers.Services;
using Moq;

namespace EnsekTestAnswers.Tests.Services;

public class AccountValidationRuleTests
{
    private readonly Mock<IAccountRepository> _mockAccountRepository;
    private readonly AccountValidationRule _validationRule;

    public AccountValidationRuleTests()
    {
        _mockAccountRepository = new Mock<IAccountRepository>();
        _validationRule = new AccountValidationRule(_mockAccountRepository.Object);
    }

    [Fact]
    public async Task ValidateAsync_InvalidAccountIdFormat_ReturnsError()
    {
        var reading = new MeterReadingDto { AccountId = "abc" };
        var validReadings = new List<MeterReading>();

        var result = await _validationRule.ValidateAsync(reading, validReadings);

        Assert.Contains("Invalid AccountId format: abc", result.ToString());
    }

    [Fact]
    public async Task ValidateAsync_AccountNotFound_ReturnsError()
    {
        var reading = new MeterReadingDto { AccountId = "123" };
        _mockAccountRepository.Setup(repo => repo.GetByAccountIdAsync(123)).ReturnsAsync((Account)null);
        var validReadings = new List<MeterReading>();

        var result = await _validationRule.ValidateAsync(reading, validReadings);

        Assert.Contains("Account not found: 123", result.ToString());
    }

    [Fact]
    public async Task ValidateAsync_AccountFound_ReturnsNoError()
    {
        var reading = new MeterReadingDto { AccountId = "123" };
        var account = new Account();
        _mockAccountRepository.Setup(repo => repo.GetByAccountIdAsync(123)).ReturnsAsync(account);
        var validReadings = new List<MeterReading>();

        var result = await _validationRule.ValidateAsync(reading, validReadings);

        Assert.Empty(result.ToString());
    }
}


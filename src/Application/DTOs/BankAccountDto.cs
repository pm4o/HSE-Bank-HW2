namespace FinanceTracker.Application.DTOs;

public sealed record BankAccountDto(string Name, decimal InitialBalance) : IDomainEntityDto;
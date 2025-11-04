using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public sealed record OperationDto(
    MoneyFlowType Type,
    Guid BankAccountId,
    Guid CategoryId,
    decimal Amount,
    DateOnly Date,
    string? Description) : IDomainEntityDto;
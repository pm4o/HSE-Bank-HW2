using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public sealed record CategoryDto(MoneyFlowType Type, string Name) : IDomainEntityDto;
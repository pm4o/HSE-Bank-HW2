using FinanceTracker.Application.Abstractions;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Commands.OperationCommand;

public sealed record CreateOperation(
    MoneyFlowType Type,
    Guid AccountId,
    Guid CategoryId,
    decimal Amount,
    DateOnly Date,
    string? Description) : ICommand<Operation>, IRecalculateAfter;
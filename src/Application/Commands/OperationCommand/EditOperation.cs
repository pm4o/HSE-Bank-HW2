using FinanceTracker.Application.Abstractions;

namespace FinanceTracker.Application.Commands.OperationCommand;

public sealed record EditOperation(
    Guid OperationId,
    decimal Amount,
    DateOnly Date,
    Guid CategoryId,
    string? Description
) : ICommand<Unit>, IRecalculateAfter;
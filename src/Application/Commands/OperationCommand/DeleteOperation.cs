using FinanceTracker.Application.Abstractions;

namespace FinanceTracker.Application.Commands.OperationCommand;

public sealed record DeleteOperation(Guid OperationId) : ICommand<Unit>, IRecalculateAfter;
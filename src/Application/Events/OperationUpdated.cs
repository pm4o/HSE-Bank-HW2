namespace FinanceTracker.Application.Events;

public sealed record OperationUpdated(Guid AccountId) : IEvent;

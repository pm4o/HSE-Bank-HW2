namespace FinanceTracker.Application.Events;

public sealed record OperationDeleted(Guid AccountId) : IEvent;
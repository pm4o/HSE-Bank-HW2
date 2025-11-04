namespace FinanceTracker.Application.Events;

public sealed record OperationCreated(Guid AccountId) : IEvent;
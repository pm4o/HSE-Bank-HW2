using FinanceTracker.Application.Abstractions;

namespace FinanceTracker.Application.Commands.AccountCommand;

public sealed record EditAccount(Guid AccountId, string NewName) : ICommand<Unit>;
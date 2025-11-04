using FinanceTracker.Application.Abstractions;

namespace FinanceTracker.Application.Commands.AccountCommand;

public sealed record DeleteAccount(Guid AccountId) : ICommand<Unit>;
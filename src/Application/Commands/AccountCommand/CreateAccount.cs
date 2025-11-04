using FinanceTracker.Application.Abstractions;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Commands.AccountCommand;

public sealed record CreateAccount(string Name, decimal InitialBalance) : ICommand<BankAccount>;
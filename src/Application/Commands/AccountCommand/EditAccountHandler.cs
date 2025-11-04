using FinanceTracker.Application.Abstractions;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Commands.AccountCommand;

public sealed class EditAccountHandler : ICommandHandler<EditAccount, Unit>
{
    private readonly IRepository<BankAccount> _accounts;
    public EditAccountHandler(IRepository<BankAccount> accounts) => _accounts = accounts;

    public Unit Handle(EditAccount cmd)
    {
        var acc = _accounts.GetById(cmd.AccountId)
                  ?? throw new InvalidOperationException("Account not found.");
        var name = (cmd.NewName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(cmd.NewName));

        acc.Rename(name);
        _accounts.Update(acc);
        return new Unit();
    }
}
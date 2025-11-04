using FinanceTracker.Application.Abstractions;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Commands.AccountCommand;

public sealed class DeleteAccountHandler : ICommandHandler<DeleteAccount, Unit>
{
    private readonly IRepository<BankAccount> _accounts;
    private readonly IRepository<Operation> _operations;

    public DeleteAccountHandler(IRepository<BankAccount> accounts, IRepository<Operation> operations)
    { _accounts = accounts; _operations = operations; }

    public Unit Handle(DeleteAccount cmd)
    {
        var acc = _accounts.GetById(cmd.AccountId)
                  ?? throw new InvalidOperationException("Account not found.");

        var hasOps = _operations.List().Any(o => o.BankAccountId == acc.Id);
        if (hasOps)
            throw new InvalidOperationException("Нельзя удалить счёт с существующими операциями.");

        _accounts.Delete(acc.Id);
        return new Unit();
    }
}
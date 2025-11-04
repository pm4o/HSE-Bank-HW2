using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Services;

public sealed class Recalculator : IRecalculator
{
    private readonly IRepository<BankAccount> _accounts;
    private readonly IRepository<Operation> _operations;

    public Recalculator(IRepository<BankAccount> accounts, IRepository<Operation> operations)
    {
        _accounts = accounts;
        _operations = operations;
    }

    public void RecalculateAll()
    {
        var accounts = _accounts.List().ToDictionary(a => a.Id, a => a);

        foreach (var account in accounts)
        {
            account.Value.Apply(-account.Value.Balance);
        }
        foreach (var op in _operations.List())
        {
            var sign = op.Type == MoneyFlowType.Income ? 1m : -1m;
            accounts[op.BankAccountId].Apply(sign * op.Amount);
        }
    }
}

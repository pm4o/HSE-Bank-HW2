using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Domain.Entities;

public sealed class BankAccount : IAcceptVisitor
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public decimal Balance { get; private set; }

    internal BankAccount(Guid id, string name, decimal initialBalance)
    {
        Id = id;
        Name = name.Trim();
        Balance = initialBalance;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("New name cannot be empty.", nameof(newName));
        Name = newName.Trim();
    }

    public void Apply(decimal delta)
    {
        Balance += delta;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
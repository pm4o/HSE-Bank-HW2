using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Domain.Entities;

public sealed class Category : IAcceptVisitor
{
    public Guid Id { get; }
    public MoneyFlowType Type { get; }
    public string Name { get; private set; }

    internal Category(Guid id, MoneyFlowType type, string name)
    {
        Id = id;
        Type = type;
        Name = name.Trim();
    }

    public void Rename(string newName)
    {
        Name = newName.Trim();
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
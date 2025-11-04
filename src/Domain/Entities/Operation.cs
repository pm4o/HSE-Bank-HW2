using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Domain.Entities;

public sealed class Operation : IAcceptVisitor
{
    public Guid Id { get; }
    public MoneyFlowType Type { get; }
    public Guid BankAccountId { get; }
    public Guid CategoryId { get; private set; }
    public decimal Amount { get; private set; }
    public DateOnly Date { get; private set; }
    public string? Description { get; private set; }

    internal Operation(Guid id, MoneyFlowType type, Guid bankAccountId, Guid categoryId, decimal amount, DateOnly date,
        string? description)
    {
        Id = id;
        Type = type;
        BankAccountId = bankAccountId;
        CategoryId = categoryId;
        Amount = amount;
        Date = date;
        Description = description;
    }
    
    public void Update(decimal amount, DateOnly date, Guid categoryId, string? note)
    {
        Amount = amount;
        Date = date;
        CategoryId = categoryId;
        Description = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
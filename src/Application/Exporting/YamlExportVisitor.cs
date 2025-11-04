using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Exporting;

public sealed class YamlExportVisitor : IVisitor
{
    private readonly List<BankAccount> _accounts = new();
    private readonly List<Category> _categories = new();
    private readonly List<Operation> _operations = new();

    public void Visit(BankAccount account)
    {
        _accounts.Add(account);
    }

    public void Visit(Category category)
    {
        _categories.Add(category);
    }

    public void Visit(Operation operation)
    {
        _operations.Add(operation);
    }

    public void Save(string filePath)
    {
        using var writer = new StreamWriter(filePath);
        writer.WriteLine("accounts:");
        foreach (var a in _accounts)
        {
            writer.WriteLine($"  - id: {a.Id}");
            writer.WriteLine($"    name: {a.Name}");
            writer.WriteLine($"    balance: {a.Balance}");
        }

        writer.WriteLine("categories:");
        foreach (var c in _categories)
        {
            writer.WriteLine($"  - id: {c.Id}");
            writer.WriteLine($"    type: {c.Type}");
            writer.WriteLine($"    name: {c.Name}");
        }

        writer.WriteLine("operations:");
        foreach (var o in _operations)
        {
            writer.WriteLine($"  - id: {o.Id}");
            writer.WriteLine($"    type: {o.Type}");
            writer.WriteLine($"    accountId: {o.BankAccountId}");
            writer.WriteLine($"    categoryId: {o.CategoryId}");
            writer.WriteLine($"    amount: {o.Amount}");
            writer.WriteLine($"    date: {o.Date}");
            writer.WriteLine($"    description: {o.Description}");
        }
    }
}
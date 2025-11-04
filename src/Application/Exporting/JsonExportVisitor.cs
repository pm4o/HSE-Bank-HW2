using System.Text.Json;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Exporting;

public sealed class JsonExportVisitor : IVisitor
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
        var dto = new { Accounts = _accounts, Categories = _categories, Operations = _operations };
        var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }
}
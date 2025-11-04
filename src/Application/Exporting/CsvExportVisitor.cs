using System.Text;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Exporting;

public sealed class CsvExportVisitor : IVisitor
{
    private readonly StringBuilder _accounts = new("id;name;balance\n");
    private readonly StringBuilder _categories = new("id;type;name\n");
    private readonly StringBuilder _operations = new("id;type;accountId;categoryId;amount;date;description\n");

    public void Visit(BankAccount account)
    {
        _accounts.AppendLine($"{account.Id};{account.Name};{account.Balance}");
    }

    public void Visit(Category category)
    {
        _categories.AppendLine($"{category.Id};{category.Type};{category.Name}");
    }

    public void Visit(Operation operation)
    {
        _operations.AppendLine(
            $"{operation.Id};{operation.Type};{operation.BankAccountId};{operation.CategoryId};{operation.Amount};{operation.Date};{operation.Description}");
    }

    public void Save(string directoryPath)
    {
        Directory.CreateDirectory(directoryPath);
        File.WriteAllText(Path.Combine(directoryPath, "accounts.csv"), _accounts.ToString());
        File.WriteAllText(Path.Combine(directoryPath, "categories.csv"), _categories.ToString());
        File.WriteAllText(Path.Combine(directoryPath, "operations.csv"), _operations.ToString());
    }
}
using FinanceTracker.Application.Abstractions;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Exporting.Strategies;

public sealed class JsonExportStrategy : IExportStrategy
{
    public string Extension => ".json";

    public void Export(IEnumerable<BankAccount> accounts, IEnumerable<Category> categories,
        IEnumerable<Operation> operations, string targetPath)
    {
        var visitor = new JsonExportVisitor();
        
        foreach (var a in accounts) a.Accept(visitor);
        foreach (var c in categories) c.Accept(visitor);
        foreach (var o in operations) o.Accept(visitor);
        visitor.Save(targetPath);
    }
}
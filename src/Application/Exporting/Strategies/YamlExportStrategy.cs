using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Exporting.Strategies;

public sealed class YamlExportStrategy : IExportStrategy
{
    public string Extension => ".yaml";

    public void Export(IEnumerable<BankAccount> accounts, IEnumerable<Category> categories,
        IEnumerable<Operation> operations, string targetPath)
    {
        var visitor = new YamlExportVisitor();
        
        foreach (var a in accounts) a.Accept(visitor);
        foreach (var c in categories) c.Accept(visitor);
        foreach (var o in operations) o.Accept(visitor);
        visitor.Save(targetPath);
    }
}
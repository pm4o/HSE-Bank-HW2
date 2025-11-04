using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Exporting.Strategies;

public interface IExportStrategy
{
    void Export(IEnumerable<BankAccount> accounts, IEnumerable<Category> categories, IEnumerable<Operation> operations,
        string targetPath);

    string Extension { get; }
}
using FinanceTracker.Application.Abstractions;
using FinanceTracker.Application.Importing;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Commands.ImportCommand;

public sealed class ImportSnapshotHandler : ICommandHandler<ImportSnapshot, Unit>
{
    private readonly IRepository<BankAccount> _accounts;
    private readonly IRepository<Category> _categories;
    private readonly IRepository<Operation> _operations;
    private readonly IEnumerable<SnapshotImporterBase> _importers;

    public ImportSnapshotHandler(IEnumerable<SnapshotImporterBase> importers,
        IRepository<BankAccount> accounts,
        IRepository<Category> categories,
        IRepository<Operation> operations)
    {
        _importers = importers;
        _accounts = accounts;
        _categories = categories;
        _operations = operations;
    }

    public Unit Handle(ImportSnapshot cmd)
    {
        var ext = Path.GetExtension(cmd.Path).ToLowerInvariant();

        var importer = _importers.FirstOrDefault(i =>
            i.SupportedExtensions.Any(e =>
                string.Equals(e, ext, StringComparison.OrdinalIgnoreCase)));

        if (importer is null)
            throw new InvalidOperationException($"Импортёр для '{ext}' не найден.");

        importer.Import(cmd.Path, _accounts, _categories, _operations);
        return new Unit();
    }
}
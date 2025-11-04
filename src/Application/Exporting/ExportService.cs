using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Exporting;

using Strategies;

public sealed class ExportService
{
    private readonly Dictionary<string, IExportStrategy> _strategies;

    public ExportService(IEnumerable<IExportStrategy> strategies)
    {
        _strategies = strategies.ToDictionary(s => s.Extension, s => s, StringComparer.OrdinalIgnoreCase);
    }

    public void Export(string path, IEnumerable<BankAccount> accounts, IEnumerable<Category> categories,
        IEnumerable<Operation> operations)
    {
        var ext = Path.GetExtension(path);
        if (!_strategies.TryGetValue(ext, out var strategy))
            throw new NotSupportedException(
                $"Unknown export extension: {ext}. Supported: {string.Join(", ", _strategies.Keys)}");
        strategy.Export(accounts, categories, operations, path);
    }
}
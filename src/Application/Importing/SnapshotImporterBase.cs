using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Importing;

public abstract class SnapshotImporterBase
{
    protected abstract (IEnumerable<BankAccount> Accounts,
        IEnumerable<Category> Categories,
        IEnumerable<Operation> Operations) ParseCore(string rawContent);

    public void Import(string path, IRepository<BankAccount> accounts, IRepository<Category> categories,
        IRepository<Operation> operations)
    {
        if (accounts is null) { throw new ArgumentNullException(nameof(accounts)); }
        if (categories is null) { throw new ArgumentNullException(nameof(categories)); }
        if (operations is null) { throw new ArgumentNullException(nameof(operations)); }

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Import path is empty.", nameof(path));
        }

        var fullPath = Path.GetFullPath(path);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"Import file not found: {fullPath}", fullPath);
        }
        
        string raw;
        try
        {
            raw = File.ReadAllText(fullPath);
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to read import file: {fullPath}", ex);
        }
        
        var (accs, cats, ops) = ParseCore(raw);

        foreach (var a in accs) accounts.Upsert(a);
        foreach (var c in cats) categories.Upsert(c);
        foreach (var o in ops) operations.Upsert(o);
    }
    
    protected virtual IEnumerable<string> GetSupportedExtensions() => Array.Empty<string>();

    public IEnumerable<string> SupportedExtensions => GetSupportedExtensions();

    public bool CanHandleExtension(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
            return false;

        return GetSupportedExtensions()
            .Contains(extension, StringComparer.OrdinalIgnoreCase);
    }

    private bool IsSupportedExtension(string? ext)
    {
        if (string.IsNullOrWhiteSpace(ext)) { return false; }
        return GetSupportedExtensions().Contains(ext, StringComparer.OrdinalIgnoreCase);
    }
}
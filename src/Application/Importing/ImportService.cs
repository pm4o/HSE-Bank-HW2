using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Importing
{
    public sealed class ImportService
    {
        private readonly IReadOnlyList<SnapshotImporterBase> _importers;

        public ImportService(
            JsonSnapshotImporter json,
            YamlSnapshotImporter yaml,
            CsvSnapshotImporter csv)
        {
            _importers = new SnapshotImporterBase[] { json, yaml, csv };
        }

        public void Import(
            string path,
            IRepository<BankAccount> accounts,
            IRepository<Category> categories,
            IRepository<Operation> operations)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Incorrect path");

            var fullPath = Path.GetFullPath(path);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"File not found: {fullPath}");

            var ext = Path.GetExtension(fullPath);
            if (string.IsNullOrWhiteSpace(ext))
                throw new InvalidOperationException("No extension found");

            var importer = _importers.FirstOrDefault(i => i.CanHandleExtension(ext));

            if (importer is null)
                throw new NotSupportedException($"Extension '{ext}' is not supported");

            importer.Import(fullPath, accounts, categories, operations);
        }
    }
}
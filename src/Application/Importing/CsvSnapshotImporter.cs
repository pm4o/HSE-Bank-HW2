using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Importing
{
    public sealed class CsvSnapshotImporter : SnapshotImporterBase
    {
        protected override IEnumerable<string> GetSupportedExtensions() => new[] { ".csv" };

        protected override (IEnumerable<BankAccount> Accounts, IEnumerable<Category> Categories, IEnumerable<Operation> Operations) ParseCore(string rawContent)
        {
            var accounts = new List<BankAccount>();
            var categories = new List<Category>();
            var operations = new List<Operation>();

            var lines = rawContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var section = "";

            foreach (var line in lines)
            {
                if (line.StartsWith("#", StringComparison.Ordinal))
                {
                    section = line.Trim().TrimStart('#').Trim().ToLowerInvariant();
                    continue;
                }

                var p = line.Split(';');
                switch (section)
                {
                    case "accounts":
                        if (p.Length < 3) throw new FormatException("Invalid accounts line.");
                        accounts.Add(new BankAccount(Guid.Parse(p[0].Trim()), p[1].Trim(), decimal.Parse(p[2].Trim())));
                        break;

                    case "categories":
                        if (p.Length < 3) throw new FormatException("Invalid categories line.");
                        categories.Add(new Category(Guid.Parse(p[0].Trim()), Enum.Parse<MoneyFlowType>(p[1].Trim(), true), p[2].Trim()));
                        break;

                    case "operations":
                        if (p.Length < 6) throw new FormatException("Invalid operations line.");
                        operations.Add(new Operation(
                            Guid.Parse(p[0].Trim()),
                            Enum.Parse<MoneyFlowType>(p[1].Trim(), true),
                            Guid.Parse(p[2].Trim()),
                            Guid.Parse(p[3].Trim()),
                            decimal.Parse(p[4].Trim()),
                            DateOnly.Parse(p[5].Trim()),
                            p.Length > 6 ? p[6] : null));
                        break;

                    default:
                        throw new FormatException("Section header is missing. Expected '# accounts', '# categories' or '# operations'.");
                }
            }

            return (accounts, categories, operations);
        }
    }
}

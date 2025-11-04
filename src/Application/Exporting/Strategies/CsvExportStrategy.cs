using FinanceTracker.Domain.Entities;
using System.Text;

namespace FinanceTracker.Application.Exporting.Strategies
{
    public sealed class CsvExportStrategy : IExportStrategy
    {
        public string Extension => ".csv";

        public void Export(IEnumerable<BankAccount> accounts, IEnumerable<Category> categories, IEnumerable<Operation> operations, string targetPath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# accounts");
            foreach (var a in accounts)
                sb.AppendLine($"{a.Id};{a.Name};{a.Balance}");
            sb.AppendLine("# categories");
            foreach (var c in categories)
                sb.AppendLine($"{c.Id};{c.Type};{c.Name}");
            sb.AppendLine("# operations");
            foreach (var o in operations)
                sb.AppendLine($"{o.Id};{o.Type};{o.BankAccountId};{o.CategoryId};{o.Amount};{o.Date:yyyy-MM-dd};{o.Description}");

            var dir = Path.GetDirectoryName(Path.GetFullPath(targetPath));
            if (string.IsNullOrEmpty(dir)) throw new ArgumentException("Invalid path.", nameof(targetPath));
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.WriteAllText(targetPath, sb.ToString());
        }
    }
}
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FinanceTracker.Application.Importing
{
    public sealed class YamlSnapshotImporter : SnapshotImporterBase
    {
        protected override IEnumerable<string> GetSupportedExtensions() => new[] { ".yaml"};

        private sealed class SnapshotYamlDto
        {
            public List<AccountYaml> Accounts { get; init; } = new();
            public List<CategoryYaml> Categories { get; init; } = new();
            public List<OperationYaml> Operations { get; init; } = new();
        }

        private sealed class AccountYaml
        {
            public string? Id { get; init; }
            public string? Name { get; init; }
            public decimal Balance { get; init; }
        }

        private sealed class CategoryYaml
        {
            public string? Id { get; init; }
            public string? Type { get; init; }
            public string? Name { get; init; }
        }

        private sealed class OperationYaml
        {
            public string? Id { get; init; }
            public string? Type { get; init; }
            public string? AccountId { get; init; }
            public string? CategoryId { get; init; }
            public decimal Amount { get; init; }
            public string? Date { get; init; }
            public string? Description { get; init; }
        }

        protected override (IEnumerable<BankAccount> Accounts,
                            IEnumerable<Category> Categories,
                            IEnumerable<Operation> Operations)
            ParseCore(string rawContent)
        {
            if (string.IsNullOrWhiteSpace(rawContent))
            {
                throw new FormatException("YAML content is empty.");
            }

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            SnapshotYamlDto dto;
            try
            {
                dto = deserializer.Deserialize<SnapshotYamlDto>(rawContent) ?? new SnapshotYamlDto();
            }
            catch (Exception ex)
            {
                throw new FormatException("Failed to deserialize YAML snapshot.", ex);
            }

            var accounts = dto.Accounts.Select(ToDomainAccount).ToList();
            var categories = dto.Categories.Select(ToDomainCategory).ToList();
            var operations = dto.Operations.Select(ToDomainOperation).ToList();

            return (accounts, categories, operations);
        }

        private static BankAccount ToDomainAccount(AccountYaml src)
        {
            var id = ParseGuid(src.Id, "accounts[].id");
            var name = RequireNonEmpty(src.Name, "accounts[].name");
            var balance = src.Balance;

            return new BankAccount(id, name, balance);
        }

        private static Category ToDomainCategory(CategoryYaml src)
        {
            var id = ParseGuid(src.Id, "categories[].id");
            var type = ParseFlowType(src.Type, "categories[].type");
            var name = RequireNonEmpty(src.Name, "categories[].name");
            return new Category(id, type, name);
        }

        private static Operation ToDomainOperation(OperationYaml src)
        {
            var id = ParseGuid(src.Id, "operations[].id");
            var type = ParseFlowType(src.Type, "operations[].type");
            var accountId = ParseGuid(src.AccountId, "operations[].accountId");
            var categoryId = ParseGuid(src.CategoryId, "operations[].categoryId");
            var amount = src.Amount;
            var date = ParseDate(src.Date, "operations[].date");
            var description = src.Description;

            return new Operation(id, type, accountId, categoryId, amount, date, description);
        }
        
        private static Guid ParseGuid(string? value, string field)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new FormatException($"Field '{field}' is required (GUID).");
            }
            if (!Guid.TryParse(value, out var id))
            {
                throw new FormatException($"Invalid GUID in field '{field}': '{value}'.");
            }
            return id;
        }

        private static MoneyFlowType ParseFlowType(string? value, string field)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new FormatException($"Field '{field}' is required (Income/Expense).");
            }
            if (!Enum.TryParse<MoneyFlowType>(value, ignoreCase: true, out var t))
            {
                throw new FormatException($"Invalid MoneyFlowType in field '{field}': '{value}'. Expected 'Income' or 'Expense'.");
            }
            return t;
        }

        private static DateOnly ParseDate(string? value, string field)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new FormatException($"Field '{field}' is required (date).");
            }
            if (!DateOnly.TryParse(value, out var d))
            {
                throw new FormatException($"Invalid date in field '{field}': '{value}'. Expected 'yyyy-MM-dd'.");
            }
            return d;
        }

        private static string RequireNonEmpty(string? value, string field)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new FormatException($"Field '{field}' cannot be empty.");
            }
            return value.Trim();
        }
    }
}

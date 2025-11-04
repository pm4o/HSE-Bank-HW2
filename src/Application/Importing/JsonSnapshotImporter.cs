using System.Text.Json;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Importing;

public sealed class JsonSnapshotImporter : SnapshotImporterBase
{
    private sealed class SnapshotDto
    {
        public List<AccountDto> Accounts { get; init; } = new();
        public List<CategoryDto> Categories { get; init; } = new();
        public List<OperationDto> Operations { get; init; } = new();
    }

    private sealed class AccountDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Balance { get; init; }
    }

    private sealed class CategoryDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public MoneyFlowType Type { get; init; }
    }

    private sealed class OperationDto
    {
        public Guid Id { get; init; }
        public MoneyFlowType Type { get; init; }
        public Guid AccountId { get; init; }
        public Guid CategoryId { get; init; }
        public decimal Amount { get; init; }
        public DateOnly Date { get; init; }
        public string? Description { get; init; }
    }

    protected override IEnumerable<string> GetSupportedExtensions() => new[] { ".json" };

    protected override (IEnumerable<BankAccount> Accounts, IEnumerable<Category> Categories, IEnumerable<Operation>
        Operations) ParseCore(string rawContent)
    {
        var dto = JsonSerializer.Deserialize<SnapshotDto>(rawContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new SnapshotDto();

        var accounts = dto.Accounts.Select(a => new BankAccount(a.Id, a.Name, a.Balance)).ToList();
        var categories = dto.Categories.Select(c => new Category(c.Id, c.Type, c.Name)).ToList();
        var operations = dto.Operations.Select(o =>
            new Operation(o.Id, o.Type, o.AccountId, o.CategoryId, o.Amount, o.Date, o.Description)).ToList();

        return (accounts, categories, operations);
    }
}

using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Facades;

public sealed class AnalyticsFacade
{
    private readonly IRepository<Operation> _operations;
    private readonly IRepository<Category> _categories;

    public AnalyticsFacade(IRepository<Operation> operations, IRepository<Category> categories)
    {
        _operations = operations;
        _categories = categories;
    }

    public decimal GetNet(DateOnly from, DateOnly to)
    {
        var filtered = _operations.List().Where(o => o.Date >= from && o.Date <= to);
        var income = filtered.Where(o => o.Type == MoneyFlowType.Income).Sum(o => o.Amount);
        var expense = filtered.Where(o => o.Type == MoneyFlowType.Expense).Sum(o => o.Amount);
        return income - expense;
    }

    public IEnumerable<(string CategoryName, decimal Sum)> GroupByCategory(DateOnly from, DateOnly to,
        MoneyFlowType type)
    {
        var categories = _categories.List().ToDictionary(c => c.Id, c => c.Name);
        var result = _operations.List()
            .Where(o => o.Date >= from && o.Date <= to && o.Type == type)
            .GroupBy(o => o.CategoryId)
            .Select(g => (categories[g.Key], g.Sum(x => x.Amount)))
            .OrderByDescending(x => x.Item2);
        return result;
    }
}
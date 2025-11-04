using FinanceTracker.Application.Abstractions;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Commands.CategoryCommand;

public sealed class DeleteCategoryHandler : ICommandHandler<DeleteCategory, Unit>
{
    private readonly IRepository<Category> _categories;
    private readonly IRepository<Operation> _operations;

    public DeleteCategoryHandler(IRepository<Category> categories, IRepository<Operation> operations)
    { _categories = categories; _operations = operations; }

    public Unit Handle(DeleteCategory cmd)
    {
        var cat = _categories.GetById(cmd.CategoryId)
                  ?? throw new InvalidOperationException("Category not found.");

        var hasOps = _operations.List().Any(o => o.CategoryId == cat.Id);
        if (hasOps)
            throw new InvalidOperationException("Нельзя удалить категорию с привязанными операциями.");

        _categories.Delete(cat.Id);
        return new Unit();
    }
}
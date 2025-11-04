using FinanceTracker.Application.Abstractions;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Commands.CategoryCommand;

public sealed class EditCategoryHandler : ICommandHandler<EditCategory, Unit>
{
    private readonly IRepository<Category> _categories;
    public EditCategoryHandler(IRepository<Category> categories) => _categories = categories;

    public Unit Handle(EditCategory cmd)
    {
        var cat = _categories.GetById(cmd.CategoryId)
                  ?? throw new InvalidOperationException("Category not found.");
        var name = (cmd.NewName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(cmd.NewName));

        cat.Rename(name);
        _categories.Update(cat);
        return new Unit();
    }
}
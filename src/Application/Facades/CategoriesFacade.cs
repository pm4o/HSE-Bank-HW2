using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Factories;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Facades;

public sealed class CategoriesFacade
{
    private readonly IRepository<Category> _repository;
    private readonly DomainFactoryResolver _resolver;

    public CategoriesFacade(IRepository<Category> repository, DomainFactoryResolver resolver)
    {
        _repository = repository;
        _resolver = resolver;
    }

    public Category Create(MoneyFlowType type, string name)
    {
        var dto = new CategoryDto(type, name);
        var category = _resolver.CategoryFactory.Create(dto);
        _repository.Add(category);
        return category;
    }

    public void Rename(Guid id, string newName)
    {
        var category = _repository.GetById(id) ?? throw new InvalidOperationException("Category not found.");
        category.Rename(newName);
        _repository.Update(category);
    }

    public void Delete(Guid id)
    {
        _repository.Delete(id);
    }

    public IReadOnlyCollection<Category> List()
    {
        return _repository.List();
    }
    
    public MoneyFlowType GetCatType(Guid id)
    {
        return _repository.GetById(id) == null ? MoneyFlowType.Expense : _repository.GetById(id)!.Type;
    }
}
using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Factories;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Facades;

public sealed class OperationsFacade
{
    private readonly IRepository<Operation> _repository;
    private readonly DomainFactoryResolver _resolver;

    public OperationsFacade(IRepository<Operation> repository, DomainFactoryResolver resolver)
    {
        _repository = repository;
        _resolver = resolver;
    }

    public Operation Create(MoneyFlowType type, Guid accountId, Guid categoryId, decimal amount, DateOnly date,
        string? description)
    {
        var dto = new OperationDto(type, accountId, categoryId, amount, date, description);
        var operation = _resolver.OperationFactory.Create(dto);
        _repository.Add(operation);
        return operation;
    }

    public void Delete(Guid id)
    {
        _repository.Delete(id);
    }

    public IReadOnlyCollection<Operation> List()
    {
        return _repository.List();
    }
}
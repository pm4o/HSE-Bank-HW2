using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Factories;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Facades;

public sealed class AccountsFacade
{
    private readonly IRepository<BankAccount> _repository;
    private readonly DomainFactoryResolver _resolver;

    public AccountsFacade(IRepository<BankAccount> repository, DomainFactoryResolver resolver)
    {
        _repository = repository;
        _resolver = resolver;
    }

    public BankAccount Create(string name, decimal initialBalance)
    {
        var dto = new BankAccountDto(name, initialBalance);
        var account = _resolver.BankAccountFactory.Create(dto);
        _repository.Add(account);
        return account;
    }

    public void Delete(Guid id)
    {
        _repository.Delete(id);
    }

    public IReadOnlyCollection<BankAccount> List()
    {
        return _repository.List();
    }
}
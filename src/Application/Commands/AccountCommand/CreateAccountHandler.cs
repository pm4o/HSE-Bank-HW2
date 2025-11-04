using FinanceTracker.Application.Abstractions;
using FinanceTracker.Application.Facades;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Commands.AccountCommand;

public sealed class CreateAccountHandler : ICommandHandler<CreateAccount, BankAccount>
{
    private readonly AccountsFacade _facade;

    public CreateAccountHandler(AccountsFacade facade)
    {
        _facade = facade;
    }

    public BankAccount Handle(CreateAccount command)
    {
        return _facade.Create(command.Name, command.InitialBalance);
    }
}
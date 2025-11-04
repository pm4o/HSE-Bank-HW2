using FinanceTracker.Application.Abstractions;
using FinanceTracker.Application.Events;
using FinanceTracker.Application.Facades;
using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Commands.OperationCommand;

public sealed class DeleteOperationHandler : ICommandHandler<DeleteOperation, Unit>
{
    private readonly IRepository<Operation> _operations;
    private readonly OperationsFacade _facade;
    private readonly EventBus _events;

    public DeleteOperationHandler(IRepository<Operation> operations, OperationsFacade facade, EventBus events)
    {
        _operations = operations;
        _facade = facade;
        _events = events;
    }

    public Unit Handle(DeleteOperation cmd)
    {
        
        var op = _operations.GetById(cmd.OperationId)
                 ?? throw new InvalidOperationException("Operation not found.");

        var accId = op.BankAccountId;
        
        _facade.Delete(cmd.OperationId);
        
        _events.Publish(new OperationDeleted(accId));
        
        return new Unit();
    }
}
using FinanceTracker.Application.Abstractions;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Events;
using FinanceTracker.Application.Factories;
using FinanceTracker.Application.Services;
using FinanceTracker.Application.Validation;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Commands.OperationCommand;

public sealed class EditOperationHandler : ICommandHandler<EditOperation, Unit>
{
    private readonly IRepository<Operation> _operations;
    private readonly EventBus _events;

    public EditOperationHandler(IRepository<Operation> operations, EventBus events)
    {
        _operations = operations;
        _events = events;
    }

    public Unit Handle(EditOperation cmd)
    {
        var op  = _operations.GetById(cmd.OperationId) ?? throw new InvalidOperationException("Operation not found.");

        var accId = op.BankAccountId;
        
        if (cmd.Amount <= 0) throw new ArgumentException("Amount must be > 0.", nameof(cmd.Amount));

        op.Update(cmd.Amount, cmd.Date, cmd.CategoryId, cmd.Description);
        _operations.Update(op);
        
        _events.Publish(new OperationUpdated(accId));

        return new Unit();
    }
}
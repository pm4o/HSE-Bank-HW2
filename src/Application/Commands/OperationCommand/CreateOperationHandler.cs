using FinanceTracker.Application.Abstractions;
using FinanceTracker.Application.Events;
using FinanceTracker.Application.Facades;
using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Commands.OperationCommand
{

    public sealed class CreateOperationHandler : ICommandHandler<CreateOperation, Operation>
    {
        private readonly OperationsFacade _facade;
        private readonly EventBus _events;

        public CreateOperationHandler(OperationsFacade facade, EventBus events)
        {
            _facade = facade;
            _events = events;
        }

        public Operation Handle(CreateOperation command)
        {
            var res =  _facade.Create(
                command.Type,
                command.AccountId,
                command.CategoryId,
                command.Amount,
                command.Date,
                command.Description);
            
            _events.Publish(new OperationCreated(command.AccountId));
            
            return res;
        }
    }
}
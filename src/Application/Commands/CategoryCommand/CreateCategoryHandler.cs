using FinanceTracker.Application.Abstractions;
using FinanceTracker.Application.Facades;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Commands.CategoryCommand
{
    public sealed class CreateCategoryHandler : ICommandHandler<CreateCategory, Category>
    {
        private readonly CategoriesFacade _facade;

        public CreateCategoryHandler(CategoriesFacade facade)
        {
            _facade = facade;
        }

        public Category Handle(CreateCategory command)
        {
            return _facade.Create(command.Type, command.Name);
        }
    }
}
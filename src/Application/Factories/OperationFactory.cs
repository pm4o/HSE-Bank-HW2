using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Validation;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Factories;

public sealed class OperationFactory : IOperationFactory
{
    private readonly IValidator<OperationDto> _validator;
    private readonly IRepository<BankAccount> _accounts;
    private readonly IRepository<Category> _categories;

    public OperationFactory(
        IValidator<OperationDto> validator,
        IRepository<BankAccount> accounts,
        IRepository<Category> categories)
    {
        _validator = validator;
        _accounts = accounts;
        _categories = categories;
    }

    public Operation Create(OperationDto dto)
    {
        _validator.Validate(dto);

        var account = _accounts.GetById(dto.BankAccountId)
                      ?? throw new InvalidOperationException("Account not found.");

        var category = _categories.GetById(dto.CategoryId)
                       ?? throw new InvalidOperationException("Category not found.");

        if (category.Type != dto.Type)
        {
            throw new InvalidOperationException("Category type does not match operation type.");
        }

        if (dto.Type == MoneyFlowType.Expense && account.Balance < dto.Amount)
        {
            throw new InvalidOperationException("Insufficient funds for expense operation.");
        }

        return new Operation(Guid.NewGuid(), dto.Type, dto.BankAccountId, dto.CategoryId, dto.Amount, dto.Date, dto.Description);
    }
}
using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Validation;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Factories;

public sealed class BankAccountFactory : IBankAccountFactory
{
    
    private readonly IValidator<BankAccountDto> _validator;

    public BankAccountFactory(IValidator<BankAccountDto> validator)
    {
        _validator = validator;
    }
    
    public BankAccount Create(BankAccountDto dto)
    {
        _validator.Validate(dto);
        return new BankAccount(Guid.NewGuid(), dto.Name.Trim(), dto.InitialBalance);
    }
}
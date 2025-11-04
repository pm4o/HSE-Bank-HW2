// src/FinanceTracker/Application/Validation/BankAccountDtoValidator.cs

using FinanceTracker.Application.DTOs;

namespace FinanceTracker.Application.Validation;

public sealed class BankAccountDtoValidator : IValidator<BankAccountDto>
{
    public void Validate(BankAccountDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name)) errors.Add("Account name cannot be empty.");
        if (dto.InitialBalance < 0m) errors.Add("Initial balance cannot be negative.");

        if (errors.Count > 0) throw new ValidationException(errors);
    }
}
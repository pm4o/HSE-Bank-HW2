using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Validation
{
    public sealed class OperationDtoValidator : IValidator<OperationDto>
    {
        public void Validate(OperationDto dto)
        {
            var errors = new List<string>();
            
                if (dto.BankAccountId == Guid.Empty)
                {
                    errors.Add("BankAccountId must be a non-empty GUID.");
                }
                if (dto.CategoryId == Guid.Empty)
                {
                    errors.Add("CategoryId must be a non-empty GUID.");
                }
                if (dto.Amount <= 0m)
                {
                    errors.Add("Amount must be greater than zero.");
                }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }
        }
    }
}
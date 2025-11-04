using FinanceTracker.Application.DTOs;

namespace FinanceTracker.Application.Validation
{
    public sealed class CategoryDtoValidator : IValidator<CategoryDto>
    {
        public void Validate(CategoryDto dto)
        {
            var errors = new List<string>();


                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    errors.Add("Category name cannot be empty.");
                }
            

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }
        }
    }
}
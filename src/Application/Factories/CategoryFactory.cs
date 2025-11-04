using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Validation;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Factories;

public sealed class CategoryFactory : ICategoryFactory
{
    private readonly IValidator<CategoryDto> _validator;

    public CategoryFactory(IValidator<CategoryDto> validator)
    {
        _validator = validator;
    }
    public Category Create(CategoryDto dto)
    {
        _validator.Validate(dto);
        
        return new Category(Guid.NewGuid(), dto.Type, dto.Name);
    }
}
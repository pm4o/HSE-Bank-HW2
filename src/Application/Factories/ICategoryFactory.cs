using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Factories;

public interface ICategoryFactory
{
    Category Create(CategoryDto dto);
}
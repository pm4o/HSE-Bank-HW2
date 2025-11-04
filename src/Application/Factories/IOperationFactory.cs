using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Factories;

public interface IOperationFactory
{
    Operation Create(OperationDto dto);
}
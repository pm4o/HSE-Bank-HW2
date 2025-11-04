
using FinanceTracker.Application.Abstractions;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Commands.CategoryCommand;
public sealed record CreateCategory(MoneyFlowType Type, string Name) : ICommand<Category>;
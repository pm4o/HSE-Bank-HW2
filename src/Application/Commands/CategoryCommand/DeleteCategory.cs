using FinanceTracker.Application.Abstractions;

namespace FinanceTracker.Application.Commands.CategoryCommand;

public sealed record DeleteCategory(Guid CategoryId) : ICommand<Unit>;
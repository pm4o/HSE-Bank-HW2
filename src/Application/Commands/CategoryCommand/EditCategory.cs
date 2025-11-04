using FinanceTracker.Application.Abstractions;

namespace FinanceTracker.Application.Commands.CategoryCommand;

public sealed record EditCategory(Guid CategoryId, string NewName) : ICommand<Unit>;
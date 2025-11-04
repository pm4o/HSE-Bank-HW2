using FinanceTracker.Application.Abstractions;
using FinanceTracker.Application.Commands.ExportCommand;

namespace FinanceTracker.Application.Commands.ImportCommand;

public sealed record ImportSnapshot(string Path) : ICommand<Unit>;
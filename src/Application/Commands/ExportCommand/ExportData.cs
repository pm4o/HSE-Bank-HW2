using FinanceTracker.Application.Abstractions;

namespace FinanceTracker.Application.Commands.ExportCommand;

public sealed record ExportData(string Path) : ICommand<Unit>;
using FinanceTracker.Application.Abstractions;

namespace FinanceTracker.Application.Commands.AnalyticsCommand;

public sealed record CalculateMonthlyNet(DateOnly From, DateOnly To) : ICommand<decimal>;
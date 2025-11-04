using FinanceTracker.Application.Abstractions;
using FinanceTracker.Application.Facades;

namespace FinanceTracker.Application.Commands.AnalyticsCommand;

public sealed class CalculateMonthlyNetHandler : ICommandHandler<CalculateMonthlyNet, decimal>
{
    private readonly AnalyticsFacade _analytics;

    public CalculateMonthlyNetHandler(AnalyticsFacade analytics)
    {
        _analytics = analytics;
    }

    public decimal Handle(CalculateMonthlyNet command)
    {
        return _analytics.GetNet(command.From, command.To);
    }
}
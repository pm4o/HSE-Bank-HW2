namespace FinanceTracker.Application.Commands.Timing;

public interface ITimingSink
{
    void Report(string operationName, TimeSpan elapsed);
}
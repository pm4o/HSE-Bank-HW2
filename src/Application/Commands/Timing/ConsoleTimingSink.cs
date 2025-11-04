using Spectre.Console;

namespace FinanceTracker.Application.Commands.Timing;

public sealed class ConsoleTimingSink : ITimingSink
{
    public void Report(string operationName, TimeSpan elapsed)
    {
        AnsiConsole.MarkupLine($"[blue]{operationName} executed in {elapsed.TotalMilliseconds:N0} ms[/]");
    }
}
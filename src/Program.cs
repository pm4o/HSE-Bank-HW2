using FinanceTracker.Application.Commands;
using FinanceTracker.Presentation;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker;

public static class Program
{
    public static void Main(string[] args)
    {
        var services = CompositionRoot.BuildServices();
        
        var menu = services.GetRequiredService<MainMenu>();
        ConsoleEncodingSetup.Set();
        menu.Run();
    }
}
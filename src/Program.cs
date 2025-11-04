using FinanceTracker.Application.Commands;
using FinanceTracker.Presentation;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        var services = CompositionRoot.BuildServices();
        var menu = services.GetRequiredService<MainMenu>();
        menu.Run();
    }
}
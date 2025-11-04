
namespace FinanceTracker.Application.Abstractions;

public interface ICommandBus
{
    TResult Send<TResult>(ICommand<TResult> command);
}


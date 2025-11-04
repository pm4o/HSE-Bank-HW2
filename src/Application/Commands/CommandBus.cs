using System.Diagnostics;
using FinanceTracker.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using FinanceTracker.Application.Commands.Timing;

namespace FinanceTracker.Application.Commands;

public sealed class CommandBus : ICommandBus
{
    private readonly IServiceProvider _sp;
    public CommandBus(IServiceProvider sp) => _sp = sp;

    public TResult Send<TResult>(ICommand<TResult> command)
    {
        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(command.GetType(), typeof(TResult));
        var handler = _sp.GetRequiredService(handlerType);
        return ((dynamic)handler).Handle((dynamic)command);
    }
}
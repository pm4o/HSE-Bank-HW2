using System.Diagnostics;
using FinanceTracker.Application.Abstractions;
using FinanceTracker.Application.Commands.Timing;

namespace FinanceTracker.Application.Commands.Decorators;

public sealed class TimingHandlerDecorator<TCommand,TResult>
    : ICommandHandler<TCommand,TResult>
    where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand,TResult> _inner;
    private readonly ITimingSink _sink;

    public TimingHandlerDecorator(ICommandHandler<TCommand, TResult> inner, ITimingSink sink)
    {
        _inner = inner; 
        _sink = sink;
    }

    public TResult Handle(TCommand command)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            return _inner.Handle(command);
        }
        finally
        {
            sw.Stop(); 
            _sink.Report(typeof(TCommand).Name, sw.Elapsed);
        }
    }
}
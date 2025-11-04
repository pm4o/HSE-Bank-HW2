using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker.Application.Events;

public sealed class EventBus
{
    private readonly IServiceProvider _sp;
    public EventBus(IServiceProvider sp) => _sp = sp;

    public void Publish<TEvent>(TEvent ev) where TEvent : IEvent
    {
        var handlers = _sp.GetServices<IEventHandler<TEvent>>();
        foreach (var h in handlers)
            h.Handle(ev);
    }
}
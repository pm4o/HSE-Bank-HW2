using FinanceTracker.Application.Services;

namespace FinanceTracker.Application.Events;

public sealed class RecalculateOnOperationEvents :
    IEventHandler<OperationCreated>,
    IEventHandler<OperationDeleted>,
    IEventHandler<OperationUpdated>
{
    private readonly IRecalculator _recalc;
    public RecalculateOnOperationEvents(IRecalculator recalc) => _recalc = recalc;

    public void Handle(OperationCreated ev) => _recalc.RecalculateAll();
    public void Handle(OperationDeleted ev) => _recalc.RecalculateAll();

    public void Handle(OperationUpdated ev) => _recalc.RecalculateAll();
}
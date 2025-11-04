namespace FinanceTracker.Domain.Interfaces;

public interface IAcceptVisitor
{
    void Accept(IVisitor visitor);
}
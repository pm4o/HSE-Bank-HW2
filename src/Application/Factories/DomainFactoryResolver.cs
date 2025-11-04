namespace FinanceTracker.Application.Factories;

public sealed class DomainFactoryResolver
{
    public IBankAccountFactory BankAccountFactory { get; }
    public ICategoryFactory CategoryFactory { get; }
    public IOperationFactory OperationFactory { get; }

    public DomainFactoryResolver(IBankAccountFactory bankAccountFactory, ICategoryFactory categoryFactory,
        IOperationFactory operationFactory)
    {
        BankAccountFactory = bankAccountFactory;
        CategoryFactory = categoryFactory;
        OperationFactory = operationFactory;
    }
}
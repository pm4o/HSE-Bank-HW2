using FinanceTracker.Application.Abstractions;
using FinanceTracker.Application.Exporting;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Commands.ExportCommand;

public sealed class ExportDataHandler : ICommandHandler<ExportData, Unit>
{
    private readonly ExportService _export;
    private readonly IRepository<BankAccount> _accounts;
    private readonly IRepository<Category> _categories;
    private readonly IRepository<Operation> _operations;

    public ExportDataHandler(
        ExportService export,
        IRepository<BankAccount> accounts,
        IRepository<Category> categories,
        IRepository<Operation> operations)
    {
        _export = export;
        _accounts = accounts;
        _categories = categories;
        _operations = operations;
    }

    public Unit Handle(ExportData command)
    {
        _export.Export(command.Path, _accounts.List(), _categories.List(), _operations.List());
        return new Unit();
    }
}
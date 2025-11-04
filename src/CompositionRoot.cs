using FinanceTracker.Application.Abstractions;
using FinanceTracker.Application.Commands;
using FinanceTracker.Application.Commands.AccountCommand;
using FinanceTracker.Application.Commands.AnalyticsCommand;
using FinanceTracker.Application.Commands.CategoryCommand;
using FinanceTracker.Application.Commands.Decorators;
using FinanceTracker.Application.Commands.ExportCommand;
using FinanceTracker.Application.Commands.ImportCommand;
using FinanceTracker.Application.Commands.OperationCommand;
using FinanceTracker.Application.Commands.Timing;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Events;
using FinanceTracker.Application.Exporting;
using FinanceTracker.Application.Exporting.Strategies;
using FinanceTracker.Application.Facades;
using FinanceTracker.Application.Factories;
using FinanceTracker.Application.Importing;
using FinanceTracker.Application.Services;
using FinanceTracker.Application.Validation;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Persistence;
using FinanceTracker.Presentation;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker;

public static class CompositionRoot
    {
        public static ServiceProvider BuildServices()
        {
            var sc = new ServiceCollection();

            sc.AddSingleton<IRepository<BankAccount>>(sp => new InMemoryRepository<BankAccount>(x => x.Id));
            sc.AddSingleton<IRepository<Category>>(sp    => new InMemoryRepository<Category>(x => x.Id));
            sc.AddSingleton<IRepository<Operation>>(sp   => new InMemoryRepository<Operation>(x => x.Id));

            sc.AddSingleton<IValidator<BankAccountDto>, BankAccountDtoValidator>();
            sc.AddSingleton<IValidator<CategoryDto>, CategoryDtoValidator>();
            sc.AddSingleton<IValidator<OperationDto>, OperationDtoValidator>();

            sc.AddSingleton<IBankAccountFactory, BankAccountFactory>();
            sc.AddSingleton<ICategoryFactory, CategoryFactory>();
            sc.AddSingleton<IOperationFactory, OperationFactory>();
            sc.AddSingleton<DomainFactoryResolver>();

            sc.AddSingleton<AccountsFacade>();
            sc.AddSingleton<CategoriesFacade>();
            sc.AddSingleton<OperationsFacade>();
            sc.AddSingleton<AnalyticsFacade>();

            sc.AddSingleton<CsvExportVisitor>();
            sc.AddSingleton<JsonExportVisitor>();
            sc.AddSingleton<YamlExportVisitor>();
            sc.AddSingleton<IExportStrategy, CsvExportStrategy>();
            sc.AddSingleton<IExportStrategy, JsonExportStrategy>();
            sc.AddSingleton<IExportStrategy, YamlExportStrategy>();
            sc.AddSingleton<ExportService>();

            sc.AddTransient<SnapshotImporterBase, JsonSnapshotImporter>();
            sc.AddTransient<SnapshotImporterBase, YamlSnapshotImporter>();
            sc.AddTransient<SnapshotImporterBase, CsvSnapshotImporter>();
            sc.AddSingleton<ImportService>();

            sc.AddTransient<CreateAccountHandler>();
            sc.AddTransient<ICommandHandler<CreateAccount, BankAccount>, CreateAccountHandler>();
            sc.AddTransient<ICommandHandler<CreateCategory, Category>, CreateCategoryHandler>();
            sc.AddTransient<ICommandHandler<CreateOperation, Operation>, CreateOperationHandler>();
            sc.AddTransient<ICommandHandler<ExportData, Unit>, ExportDataHandler>();
            sc.AddTransient<ICommandHandler<ImportSnapshot, Unit>, ImportSnapshotHandler>();
            sc.AddTransient<ICommandHandler<CalculateMonthlyNet, decimal>, CalculateMonthlyNetHandler>();
            
            sc.AddTransient<ICommandHandler<EditAccount, Unit>, EditAccountHandler>();
            sc.AddTransient<ICommandHandler<DeleteAccount, Unit>, DeleteAccountHandler>();

            sc.AddTransient<ICommandHandler<EditCategory, Unit>, EditCategoryHandler>();
            sc.AddTransient<ICommandHandler<DeleteCategory, Unit>, DeleteCategoryHandler>();

            sc.AddTransient<ICommandHandler<EditOperation, Unit>, EditOperationHandler>();
            sc.AddTransient<ICommandHandler<DeleteOperation, Unit>, DeleteOperationHandler>();

            sc.AddSingleton<IRecalculator, Recalculator>();
            sc.AddSingleton<ITimingSink, ConsoleTimingSink>();
            sc.Decorate(typeof(ICommandHandler<,>), typeof(TimingHandlerDecorator<,>));
            sc.AddSingleton<ICommandBus, CommandBus>();

            sc.AddSingleton<EventBus>();

            sc.AddTransient<IEventHandler<OperationCreated>, RecalculateOnOperationEvents>();
            sc.AddTransient<IEventHandler<OperationDeleted>, RecalculateOnOperationEvents>();
            sc.AddTransient<IEventHandler<OperationUpdated>, RecalculateOnOperationEvents>();
            
            sc.AddSingleton<MainMenu>();

            return sc.BuildServiceProvider();
        }
    }
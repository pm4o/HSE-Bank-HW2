using FinanceTracker.Application.Abstractions;
using FinanceTracker.Application.Commands.AccountCommand;
using FinanceTracker.Application.Commands.CategoryCommand;
using FinanceTracker.Application.Commands.ExportCommand;
using FinanceTracker.Application.Commands.ImportCommand;
using FinanceTracker.Application.Commands.OperationCommand;
using FinanceTracker.Application.Facades;
using FinanceTracker.Domain.Enums;
using Spectre.Console;

namespace FinanceTracker.Presentation;

public sealed class MainMenu
{
    private readonly AccountsFacade _accounts;
    private readonly CategoriesFacade _categories;
    private readonly OperationsFacade _operations;
    private readonly AnalyticsFacade _analytics;
    private readonly ICommandBus _bus;

    public MainMenu(AccountsFacade accounts, CategoriesFacade categories, OperationsFacade operations,
        AnalyticsFacade analytics, ICommandBus bus)
    {
        _accounts = accounts;
        _categories = categories;
        _operations = operations;
        _analytics = analytics;
        _bus = bus;
    }

    public void Run()
    {
        while (true)
        {
            AnsiConsole.Write(new FigletText("HSE Bank").LeftJustified().Color(Color.DeepPink4_2));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Выберите действие[/]:")
                    .HighlightStyle(new Style(Color.White, Color.DarkMagenta, Decoration.Bold))
                    .AddChoices(
                        "Создать счёт".PadRight(17),
                        "Список счетов".PadRight(17),
                        "Изменить счёт".PadRight(17),
                        "Удалить счёт".PadRight(17),
                        "Создать категорию".PadRight(17),
                        "Список категорий".PadRight(17),
                        "Изменить категорию".PadRight(17),
                        "Удалить категорию".PadRight(17),
                        "Добавить операцию".PadRight(17),
                        "Список операций".PadRight(17),
                        "Изменить операцию".PadRight(17),
                        "Удалить операцию".PadRight(17),
                        "Нетто за месяц".PadRight(17),
                        "Экспорт данных".PadRight(17),
                        "Импорт снапшота".PadRight(17),
                        "Выход".PadRight(17))
                    .PageSize(16));

            choice = choice.TrimEnd();

            if (choice == "Выход") return;

            try
            {
                if (choice == "Создать счёт")
                {
                    var name = AnsiConsole.Ask<string>("Название счёта:");
                    _bus.Send(new CreateAccount(name, 0));
                    AnsiConsole.MarkupLine("[green]Счёт создан[/]");
                    WaitKeyPress();
                }
                else if (choice == "Список счетов")
                {
                    var items = _accounts.List();
                    var table = new Table().Border(TableBorder.Rounded).Title("Счета");
                    table.AddColumn("Id");
                    table.AddColumn("Название");
                    table.AddColumn("Баланс");
                    foreach (var a in items) table.AddRow(a.Id.ToString(), a.Name, a.Balance.ToString("0.00"));
                    AnsiConsole.Write(table);
                    WaitKeyPress();
                }
                else if (choice == "Создать категорию")
                {
                    var typeStr = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Тип категории:")
                        .AddChoices("Income", "Expense"));
                    var type = Enum.Parse<MoneyFlowType>(typeStr, true);
                    var name = AnsiConsole.Ask<string>("Название категории:");
                    _bus.Send(new CreateCategory(type, name));
                    AnsiConsole.MarkupLine("[green]Категория создана[/]");
                    WaitKeyPress();
                }
                else if (choice == "Список категорий")
                {
                    var items = _categories.List();
                    var table = new Table().Border(TableBorder.Rounded).Title("Категории");
                    table.AddColumn("Id");
                    table.AddColumn("Тип");
                    table.AddColumn("Название");
                    foreach (var c in items) table.AddRow(c.Id.ToString(), c.Type.ToString(), c.Name);
                    AnsiConsole.Write(table);
                    WaitKeyPress();
                }
                else if (choice == "Добавить операцию")
                {
                    if (_accounts.List().Count == 0 || _categories.List().Count == 0)
                    {
                        AnsiConsole.MarkupLine("[yellow]Нужны хотя бы один счёт и одна категория[/]");
                    }
                    else
                    {
                        var accChoice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Счёт:")
                            .AddChoices(_accounts.List().Select(a => $"{a.Id} | {a.Name}")));
                        var catChoice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Категория:")
                            .AddChoices(_categories.List().Select(c => $"{c.Id} | {c.Name} | {c.Type}")));
                        var amount = AnsiConsole.Ask<decimal>("Сумма (> 0):");
                        var description = AnsiConsole.Ask<string>("Описание:");
                        var accId = Guid.Parse(accChoice.Split('|')[0].Trim());
                        var catId = Guid.Parse(catChoice.Split('|')[0].Trim());
                        var type = _categories.GetCatType(catId);
                        var date = DateOnly.FromDateTime(DateTime.Today);
                        _bus.Send(new CreateOperation(type, accId, catId, amount, date, description));
                        AnsiConsole.MarkupLine("[green]Операция добавлена[/]");
                    }

                    WaitKeyPress();
                }
                else if (choice == "Список операций")
                {
                    var items = _operations.List();
                    var table = new Table().Border(TableBorder.Rounded).Title("Операции");
                    table.AddColumn("Id");
                    table.AddColumn("Тип");
                    table.AddColumn("Счёт");
                    table.AddColumn("Категория");
                    table.AddColumn("Сумма");
                    table.AddColumn("Дата");
                    table.AddColumn("Описание");
                    foreach (var o in items)
                        table.AddRow(o.Id.ToString(), o.Type.ToString(), o.BankAccountId.ToString(),
                            o.CategoryId.ToString(), o.Amount.ToString("0.00"), o.Date.ToString("yyyy-MM-dd"),
                            o.Description ?? "");
                    AnsiConsole.Write(table);
                    WaitKeyPress();
                }
                else if (choice == "Нетто за месяц")
                {
                    var today = DateTime.Today;
                    var from = new DateOnly(today.Year, today.Month, 1);
                    var to = from.AddMonths(1).AddDays(-1);
                    var net = _analytics.GetNet(from, to);
                    var panel = new Panel($"[bold]{from:yyyy-MM}[/]\nНетто: [green]{net:0.00}[/]")
                        .Header("Аналитика за месяц").Border(BoxBorder.Rounded);
                    AnsiConsole.Write(panel);
                    WaitKeyPress();
                }
                else if (choice == "Экспорт данных")
                {
                    var format = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Формат экспорта:")
                        .AddChoices("JSON (.json)", "YAML (.yaml)", "CSV (.csv)"));
                    var path = format switch
                    {
                        "JSON (.json)" => AnsiConsole.Ask<string>("Путь к файлу (.json):"),
                        "YAML (.yaml)" => AnsiConsole.Ask<string>("Путь к файлу (.yaml):"),
                        "CSV (.csv)" => AnsiConsole.Ask<string>("Путь к файлу (.csv):"),
                        _ => throw new NotSupportedException()
                    };
                    _bus.Send(new ExportData(path));
                    AnsiConsole.MarkupLine("[green]Экспорт завершён[/]");
                    WaitKeyPress();
                }
                else if (choice == "Импорт снапшота")
                {
                    var path = AnsiConsole.Ask<string>("Путь к файлу импорта (.json/.yaml/.csv):");
                    _bus.Send(new ImportSnapshot(path));
                    AnsiConsole.MarkupLine("[green]Импорт завершён[/]");
                    WaitKeyPress();
                }
                else if (choice == "Изменить счёт")
                {
                    if (_accounts.List().Count == 0)
                    {
                        AnsiConsole.MarkupLine("[yellow]Нет счетов для изменения[/]");
                        WaitKeyPress();
                    }
                    else
                    {
                        var picked = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Выберите счёт:")
                                .AddChoices(_accounts.List().Select(a => $"{a.Id} | {a.Name} | {a.Balance:0.00}"))
                        );
                        var accountId = Guid.Parse(picked.Split('|')[0].Trim());
                        var newName = AnsiConsole.Ask<string>("Новое название счёта:");
                        _bus.Send(new EditAccount(accountId, newName));
                        AnsiConsole.MarkupLine("[green]Счёт обновлён[/]");
                        WaitKeyPress();
                    }
                }
                else if (choice == "Удалить счёт")
                {
                    if (_accounts.List().Count == 0)
                    {
                        AnsiConsole.MarkupLine("[yellow]Нет счетов для удаления![/]");
                        WaitKeyPress();
                    }
                    else
                    {
                        var picked = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Выберите счёт для удаления:")
                                .PageSize(10)
                                .AddChoices(_accounts.List().Select(a => $"{a.Id} | {a.Name} | {a.Balance:0.00}"))
                        );
                        var accountId = Guid.Parse(picked.Split('|')[0].Trim());

                        if (AnsiConsole.Confirm("[red]Точно удалить счёт?[/]"))
                        {
                            _bus.Send(new DeleteAccount(accountId));
                            AnsiConsole.MarkupLine("[green]Счёт удалён[/]");
                        }

                        WaitKeyPress();
                    }
                }

                else if (choice == "Изменить категорию")
                {
                    if (_categories.List().Count == 0)
                    {
                        AnsiConsole.MarkupLine("[yellow]Нет категорий для изменения[/]");
                        WaitKeyPress();
                    }
                    else
                    {
                        var picked = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Выберите категорию:")
                                .AddChoices(_categories.List().Select(c => $"{c.Id} | {c.Name} | {c.Type}"))
                        );
                        var categoryId = Guid.Parse(picked.Split('|')[0].Trim());
                        var newName = AnsiConsole.Ask<string>("Новое название категории:");
                        _bus.Send(new EditCategory(categoryId, newName));
                        AnsiConsole.MarkupLine("[green]Категория обновлена[/]");
                        WaitKeyPress();
                    }
                }
                else if (choice == "Удалить категорию")
                {
                    if (_categories.List().Count == 0)
                    {
                        AnsiConsole.MarkupLine("[yellow]Нет категорий для удаления[/]");
                        WaitKeyPress();
                    }
                    else
                    {
                        var picked = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Выберите категорию для удаления:")
                                .AddChoices(_categories.List().Select(c => $"{c.Id} | {c.Name} | {c.Type}"))
                        );
                        var categoryId = Guid.Parse(picked.Split('|')[0].Trim());

                        if (AnsiConsole.Confirm("[red]Точно удалить категорию?[/]"))
                        {
                            _bus.Send(new DeleteCategory(categoryId));
                            AnsiConsole.MarkupLine("[green]Категория удалена[/]");
                        }

                        WaitKeyPress();
                    }
                }

                else if (choice == "Изменить операцию")
                {
                    if (_operations.List().Count == 0)
                    {
                        AnsiConsole.MarkupLine("[yellow]Нет операций для изменения[/]");
                        WaitKeyPress();
                    }
                    else
                    {
                        var pickedOp = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Выберите операцию:")
                                .PageSize(10)
                                .AddChoices(_operations.List()
                                    .Select(o =>
                                        $"{o.Id} | {o.Type} | {o.Amount:0.00} | {o.Date:yyyy-MM-dd} | acc:{o.BankAccountId} cat:{o.CategoryId}"))
                        );
                        var opId = Guid.Parse(pickedOp.Split('|')[0].Trim());

                        var amount = AnsiConsole.Ask<decimal>("Новая сумма (> 0):");
                        var dateStr = AnsiConsole.Ask<string>("Новая дата (yyyy-MM-dd):");
                        if (!DateOnly.TryParse(dateStr, out var date))
                            date = DateOnly.FromDateTime(DateTime.Today);

                        var pickedCat = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Новая категория:")
                                .AddChoices(_categories.List().Select(c => $"{c.Id} | {c.Name} | {c.Type}"))
                        );
                        var categoryId = Guid.Parse(pickedCat.Split('|')[0].Trim());

                        var note = AnsiConsole.Ask<string?>("Новое описание:");

                        _bus.Send(new EditOperation(opId, amount, date, categoryId, note));

                        AnsiConsole.MarkupLine("[green]Операция обновлена[/]");
                        WaitKeyPress();
                    }
                }
                else if (choice == "Удалить операцию")
                {
                    if (_operations.List().Count == 0)
                    {
                        AnsiConsole.MarkupLine("[yellow]Нет операций для удаления[/]");
                        WaitKeyPress();
                    }
                    else
                    {
                        var pickedOp = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Выберите операцию для удаления:")
                                .PageSize(10)
                                .AddChoices(_operations.List()
                                    .Select(o =>
                                        $"{o.Id} | {o.Type} | {o.Amount:0.00} | {o.Date:yyyy-MM-dd} | acc:{o.BankAccountId} cat:{o.CategoryId}"))
                        );
                        var opId = Guid.Parse(pickedOp.Split('|')[0].Trim());

                        if (AnsiConsole.Confirm("[red]Точно удалить операцию?[/]"))
                        {
                            _bus.Send(new DeleteOperation(opId));
                            AnsiConsole.MarkupLine("[green]Операция удалена[/]");
                        }

                        WaitKeyPress();
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
                WaitKeyPress();
            }
        }
    }

    public void WaitKeyPress()
    {
        AnsiConsole.MarkupLine("Нажмите любую клавишу, чтобы продолжить...");
        Console.ReadKey();
        AnsiConsole.Clear();
    }
}
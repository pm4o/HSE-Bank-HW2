# HW-2. HSE Bank

## 1. О программе

**FinanceTracker** - консольное приложение для учёта финансов. Пользователь управляет:
- **счётами** (*BankAccount*),
- **категориями** доходов/расходов (*Category*),
- **операциями** (*Operation*).

Функции: создание/редактирование/удаление сущностей, вывод списков, добавление операций, аналитика (прибыль за месяц), импорт/экспорт (JSON/YAML/CSV), автопересчёт балансов после изменения операций, замер времени выполнения команд. UI построен на *Spectre.Console*. Данные - в *in‑memory* репозиториях.


## 2. Структура программы

### /src/Presentation

- MainMenu.cs - консольный UI на Spectre.Console. Формирует команды (создать/редактировать/удалить счета, категории, операции; импорт/экспорт; аналитика), отправляет их через ICommandBus.

### /src/Application
#### Abstractions

- ICommand<TResult>, ICommandHandler<TCommand,TResult>, ICommandBus - каркас команды.

- IRecalculateAfter - маркер/абстракция для сценариев, после которых нужен пересчёт.

#### Commands

- CommandBus - резолвит ICommandHandler<,> из DI и вызывает Handle.

- Подпапки AccountCommand, CategoryCommand, OperationCommand, AnalyticsCommand, ImportCommand, ExportCommand - по файлу команды и по файлу хэндлера для каждого сценария.

- Decorators/TimingHandlerDecorator.cs - измерение времени для любой команды.

- Timing/ - ITimingSink и ConsoleTimingSink, куда уходит измерение времени.

#### DTOs

- BankAccountDto, CategoryDto, OperationDto и IDomainEntityDto для транспортировки данных между слоями.

#### Events

- EventBus - простая шина: Publish<TEvent>(...) -> вызвать всех зарегистрированных IEventHandler<TEvent>.

- События: OperationCreated, OperationUpdated, OperationDeleted.

- Обработчик: RecalculateOnOperationEvents вызывает IRecalculator.RecalculateAll().

#### Exporting

- ExportService - точка входа экспорта: выбирает стратегию по расширению.

- Strategies/ - IExportStrategy + реализации под JSON/YAML/CSV.

- JsonExportVisitor/YamlExportVisitor/CsvExportVisitor - обход доменных объектов и построение выходной модели/текста.

#### Facades

- AccountsFacade, CategoriesFacade, OperationsFacade, AnalyticsFacade - объединяют операции над данными в единые высокоуровневые методы.

#### Factories

- BankAccountFactory, CategoryFactory, OperationFactory - централизованное создание доменных объектов из DTO + валидация.

- DomainFactoryResolver - удобный доступ к фабрикам.

#### Importing

- SnapshotImporterBase - общий алгоритм импорта (Template Method).

- JsonSnapshotImporter, CsvSnapshotImporter, YamlSnapshotImporter - конкретные реализации со своими парсерами.

#### Services

- IRecalculator, Recalculator - сервис пересчёта балансов на основе операций.

- DateRange - утилита для диапазона дат, используется в аналитике/фильтрации.

#### Validation

- IValidator<T>, ValidationException, CompositeValidator<T>, конкретные валидаторы DTO (BankAccountDtoValidator, CategoryDtoValidator, OperationDtoValidator).

### /src/Domain
#### Entities

- BankAccount, Category, Operation - доменные классы. Реализуют IAcceptVisitor для экспорта.

- Параметры инкапсулированы (изменение баланса через метод, безопасное переименование).

#### Enums

- MoneyFlowType - тип операции (доход/расход).

#### Interfaces

- IRepository<T> - абстракция хранилища.

- IAcceptVisitor, IVisitor - интерфейсы визитора для экспорта.

### /src/Infrastructure
#### Persistence

- InMemoryRepository<T> - реализация IRepository<T> на словаре.

### Корень /src

- CompositionRoot.cs - регистрация DI: репозитории, валидаторы, фабрики, фасады, хэндлеры, декоратор времени, шина команд, события.

- Program.cs - настройка кодировок консоли, сборка контейнера, запуск MainMenu.

- ConsoleEncodingSetup.cs - настройка энкодинга консоли.

## 3. Реализованные паттерны    

### 1. Facade

- Где: Application/Facades/
- AccountsFacade, CategoriesFacade, OperationsFacade, AnalyticsFacade.
- Когда: каждый сценарий из хэндлеров команд вместо прямой работы с несколькими репозиториями и сервисами обращается к фасаду.
- Зачем: фасады прячут как исполняется операция (какие репозитории дернуть, валидацию и т.д.), давая простой API для приложения. Это снижает связность между хэндлерами, инфраструктурой, валидаторами.

### 2. Command

- Где: Application/Abstractions/ICommand<T>, ICommandHandler<TCommand,TResult>, конкретные команды/хэндлеры в Application/Commands/, шина CommandBus.
- Когда: любой пользовательский сценарий (CRUD, экспорт, аналитика) оформлен отдельной командой.
- Зачем: изоляция сценариев, единый протокол исполнения, возможность декорировать исполнение, а значит гибкая и удобная расширяемость.

### 3. Decorator

- Где: Application/Commands/Decorators/TimingHandlerDecorator.cs, регистрация в CompositionRoot через Scutor.Decorate(...); замер времени - ITimingSink/ConsoleTimingSink.
- Когда: при каждом вызове ICommandBus.Send резолвится хэндлер, вокруг него оборачивается декоратор, который измеряет время выполнения команды.
- Зачем: добавляем функциональность без изменения кода хэндлеров. Легко подключить и другие декораторы, опять же обеспечивая гибкую расширяемость.

### 4. Template Method

- Где: Application/Importing/SnapshotImporterBase + реализации JsonSnapshotImporter, CsvSnapshotImporter, YamlSnapshotImporter.
- Когда: при импорте из файла: общий алгоритм (проверка расширения -> чтение -> парсинг) определён в базовом классе, конкретика парсинга - в абстрактном ParseCore.
- Зачем: устраняет дублирование между форматами, позволяет легко добавить новый формат, реализовав только отличающуюся.

### 5. Visitor

- Где: Domain/Interfaces/IVisitor, IAcceptVisitor; реализации посетителей: JsonExportVisitor, CsvExportVisitor, YamlExportVisitor; доменные сущности (BankAccount, Category, Operation) реализуют IAcceptVisitor.Accept.
- Когда: при экспорте данных стратегии инициируют обход доменных объектов через визитора, который собирает корректное представление данных для целевого формата.
- Зачем: отделяет структуру домена от экспорта, не засоряя доменные классы знанием о форматах экспорта. Удобно расширять программу новыми операциями, не трогая домен.

### 6. Factory

- Где: Application/Factories/ (BankAccountFactory, CategoryFactory, OperationFactory, + DomainFactoryResolver).
- Когда: при создании доменных сущностей из DTO/команд - всегда через фабрики, которые используют валидаторы DTO.
- Зачем: единая точка создания объектов + валидация параметров -> отсутствие дублирования проверок, контролируемое появление объектов в валидном состоянии.

### 7. Strategy

- Где: Application/Exporting/Strategies/ (IExportStrategy, JsonExportStrategy, YamlExportStrategy, CsvExportStrategy) и ExportService, выбирающий стратегию по расширению файла.
- Когда: при экспорте ExportService.Export(...) смотрит расширение и подбирает нужную стратегию.
- Зачем: взаимозаменяемые алгоритмы экспорта; легко добавить новый формат - просто зарегистрировать новую стратегию.

### 8. Observer

- Где: Application/Events/EventBus, события OperationCreated/Updated/Deleted, обработчик RecalculateOnOperationEvents, сервис IRecalculator/Recalculator.
- Когда: после создания/изменения/удаления операции хэндлер публикует событие в EventBus, а подписчик (RecalculateOnOperationEvents) реагирует пересчётом балансов (Recalculator.RecalculateAll).
- Зачем: размыкает связность - CRUD-сценарии не знают, кто захочет на них среагировать. Можно добавлять новые обработчики без изменений в исходных сценариях.

## 4. Соблюдение принципов SOLID

### S (Single Responsibility).

- Facade - инкапсулируют сценарную логику в своей области (аккаунты, категории, операции, аналитика).

- Factory - только создание и валидация(посредством валидаторов) входных данных.

- Recalculator - отвечает исключительно за пересчёт балансов из операций.

- CommandBus - маршрутизация команд.

- TimingHandlerDecorator - только измерение времени.

### O (Open/Closed).

- Легко добавлять новые форматы импорта/экспорта (новые подклассы/стратегии/визиторы), декораторы команд, хэндлеры событий - без изменения существующего кода клиентов.

### L (Liskov Substitution).

- Везде реализация через абстракции (IRepository<T>, ICommandHandler<,>, IExportStrategy, IVisitor, IValidator<T>). Подстановочность соблюдается.

### I (Interface Segregation).

- Небольшие, целевые интерфейсы (ICommandBus, ICommandHandler<,>, IRecalculator, ITimingSink, IValidator<T>, IExportStrategy). Клиенты не зависят от лишнего.

### D (Dependency Inversion).

- Зависимости настраиваются через DI-контейнер (CompositionRoot). В коде верхнего уровня используются абстракции, а не конкретные классы. Декоратор подключается через Scrutor.Decorate.

## 5. Соблюдение принципов GRASP

### High Cohesion.

- Доменные классы содержат только доменную логику (переименование, применение изменения баланса).

- Фасады собирают кусочки в законченный сценарий.

- Импорт/экспорт - каждая часть отвечает за свою стадию.

### Low Coupling.

- UI знает только про ICommandBus.

- Хэндлеры не знают, какие именно репозитории/сервисы стоят за фасадами.

- Пересчёт привязан к событиям, а не к CRUD-методам напрямую.

### Controller.

- Роль контроллера (входная точка приложения) - CommandBus. Он маршрутизирует запросы из UI на нужные хэндлеры.

### Information Expert.

- Методы, изменяющие баланс - в BankAccount.

- Логика группировки по категориям - в AnalyticsFacade, у которого есть доступ к операциям и категориям.
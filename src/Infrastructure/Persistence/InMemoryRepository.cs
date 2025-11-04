using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Infrastructure.Persistence;

public sealed class InMemoryRepository<T> : IRepository<T> where T : class
{
    private readonly Dictionary<Guid, T> _items = new();
    private readonly Func<T, Guid> _idSelector;

    public InMemoryRepository(Func<T, Guid> idSelector)
    {
        _idSelector = idSelector;
    }

    public T? GetById(Guid id)
    {
        return _items.TryGetValue(id, out var value) ? value : null;
    }

    public IReadOnlyCollection<T> List()
    {
        return _items.Values.ToList();
    }

    public void Add(T entity)
    {
        _items.Add(_idSelector(entity), entity);
    }

    public void Update(T entity)
    {
        _items[_idSelector(entity)] = entity;
    }

    public void Upsert(T entity)
    {
        _items[_idSelector(entity)] = entity;
    }

    public void Delete(Guid id)
    {
        _items.Remove(id);
    }
}
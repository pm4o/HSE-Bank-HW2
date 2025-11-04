using System;
using System.Collections.Generic;

namespace FinanceTracker.Domain.Interfaces;

public interface IRepository<T>
{
    T? GetById(Guid id);
    IReadOnlyCollection<T> List();
    void Add(T entity);
    void Update(T entity);
    void Upsert(T entity);
    void Delete(Guid id);
}
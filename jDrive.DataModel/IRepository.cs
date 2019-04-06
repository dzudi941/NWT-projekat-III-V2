using System;
using System.Collections.Generic;

namespace jDrive.DomainModel
{
    public interface IRepository<T> : IDisposable where T : class
    {
        T GetByID(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> Table { get; }
        IEnumerable<T> Find(ISpecification<T> specification, params string[] includes);
    }
}

using jDrive.Services.Specifications;
using System;
using System.Collections.Generic;

namespace jDrive.Services.Services
{
    public interface IRepository<T> : IDisposable where T : class
    {
        T GetByID(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> Table { get; }
        IEnumerable<T> Find(Specification<T> specification);
    }
}

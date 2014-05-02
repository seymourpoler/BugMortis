using System;
using System.Collections.Generic;

namespace BugMortis.Data
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Guid Insert(T entity);
        T GetById(Guid id);
        void Delete(Guid id);
        void Update(T entity);
    }
}

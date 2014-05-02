using System;
using System.Collections.Generic;

namespace BugMortis.Domain
{
    public interface IService<T> where T : class
    {
        bool IsValid(T entity, out List<string> errorMessages);
        IEnumerable<T> GetAll();
        T GetById(Guid Id);
        Guid Insert(T entity);
        void Update(T entity);
        void Delete(Guid id);
    }
}

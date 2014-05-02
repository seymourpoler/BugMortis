using System;
using System.Collections.Generic;

namespace BugMortis.Data
{
    public interface IDataMasterRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
    }
}

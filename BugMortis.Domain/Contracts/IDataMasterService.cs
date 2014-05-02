using System;
using System.Collections.Generic;

namespace BugMortis.Domain
{
    public interface IDataMasterService<T> where T : class
    {
        IEnumerable<T> GetAll();
    }
}

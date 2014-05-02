using System;
using System.Collections.Generic;
using BugMortis.Data.Entity;

namespace BugMortis.Data.SqlServer.Repositories
{
    public class PriorityRepository : BaseRepository, IDataMasterRepository<Priority>
    {
        public PriorityRepository(DataBaseContext context)
            : base(context)
        { }

        public IEnumerable<Priority> GetAll()
        {
            return _db.Priorities;
        }
    }
}

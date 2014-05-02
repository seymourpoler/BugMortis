using System;
using System.Collections.Generic;
using BugMortis.Data;
using BugMortis.Data.Entity;

namespace BugMortis.Data.SqlServer.Repositories
{
    public class StatusRepository: BaseRepository, IDataMasterRepository<Status>
    {
        public StatusRepository(DataBaseContext context)
            : base(context)
        { }

        public IEnumerable<Status> GetAll()
        {
            return _db.Status;
        }
    }
}

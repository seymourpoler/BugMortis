using System.Linq;
using System.Collections.Generic;
using BugMortis.Data;
using BugMortis.Data.Entity;
using BugMortis.Domain.Entity;
using BugMortis.Data.SqlServer.Repositories;

namespace BugMortis.Domain
{
    public class StatusService : IDataMasterService<Domain.Entity.Status>
    {
        private IDataMasterRepository<Data.Entity.Status> _repository;

        public StatusService(IDataMasterRepository<Data.Entity.Status> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Domain.Entity.Status> GetAll()
        {
            var data = _repository.GetAll();
            return data.Select(MapToDomain);
        }

        public static Domain.Entity.Status MapToDomain(Data.Entity.Status status)
        {
            return new Domain.Entity.Status{
                IdStatus = status.IdStatus,
                Name = status.Name
            };
        }
    }
}

using System.Linq;
using System.Collections.Generic;
using BugMortis.Data;
using BugMortis.Data.Entity;
using BugMortis.Domain.Entity;

namespace BugMortis.Domain
{
    public class PriorityService : IDataMasterService<Domain.Entity.Priority>
    {
        private IDataMasterRepository<Data.Entity.Priority> _repository;

        public PriorityService(IDataMasterRepository<Data.Entity.Priority> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Domain.Entity.Priority> GetAll()
        {
            var data = _repository.GetAll();
            return data.Select(MapToDomain);
        }

        public static Domain.Entity.Priority MapToDomain(Data.Entity.Priority prioriry)
        {
            return new Domain.Entity.Priority { 
                IdPriority = prioriry.IdPriority, 
                Name = prioriry.Name 
            };
        }
    }
}

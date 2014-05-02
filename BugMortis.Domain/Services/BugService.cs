using System;
using System.Linq;
using System.Collections.Generic;
using BugMortis.Domain.Entity;
using BugMortis.Data;
using BugMortis.Data.SqlServer;

namespace BugMortis.Domain
{
    public class BugService : IService<Bug>
    {
        private IRepository<Data.Entity.Bug> _bugRepository;
        public BugService(IRepository<Data.Entity.Bug> bugRepository)
        {
            _bugRepository = bugRepository;
        }

        public bool IsValid(Domain.Entity.Bug bug, out List<string> errorMessages)
        {
            var result = true;
            errorMessages = new List<string>();

            if (bug == null)
            {
                errorMessages.Add("the bug cannot be null");
                return false;
            }
            if (string.IsNullOrWhiteSpace(bug.Name))
            {
                result = false;
                errorMessages.Add("the name of the bug cannot be empty");
            }
            if(bug.IdPriority == null)
            {
                result = false;
                errorMessages.Add("the priority of the bug cannot be null");
            }
            if(bug.IdPriority == Guid.Empty)
            {
                result = false;
                errorMessages.Add("the priority of the bug cannot be a guid empty");
            }
            if(bug.IdProject == null)
            {
                result = false;
                errorMessages.Add("the id of the project of the bug cannot be null");
            }
            if(bug.IdProject == Guid.Empty)
            {
                result = false;
                errorMessages.Add("the id pf the project of the bug cannot be a guid empty");
            }

            return result;
        }

        public Domain.Entity.Bug GetById(Guid idBug)
        {
            var dataBug = _bugRepository.GetById(idBug);
            return MapToDomain(dataBug);
        }

        public IEnumerable<Domain.Entity.Bug> GetAll()
        {
            var dataBugs = _bugRepository.GetAll();
            return dataBugs.Select(bug => MapToDomain(bug));
        }

        public Guid Insert(Domain.Entity.Bug bug)
        {
            var dataBug = MapToData(bug);
            return _bugRepository.Insert(dataBug);
        }

        public void Update(Domain.Entity.Bug bug)
        {
            var dataBug = MapToData(bug);
            _bugRepository.Update(dataBug);
        }

        public void Delete(Guid idBug)
        {
            _bugRepository.Delete(idBug);
        }

        public static Domain.Entity.Bug MapToDomain(Data.Entity.Bug bug)
        {
            var result = new Domain.Entity.Bug();
            result.IdBug = bug.IdBug;
            result.Name = bug.Name;
            return result;
        }

        public static Data.Entity.Bug MapToData(Domain.Entity.Bug bug)
        {
            var result = new Data.Entity.Bug();
            result.IdBug = bug.IdBug;
            result.Name = bug.Name;
            return result;
        }
    }
}

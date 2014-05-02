using System;
using System.Linq;
using System.Collections.Generic;
using BugMortis.Data;
using BugMortis.Data.Entity;
using BugMortis.Data.SqlServer.Repositories;
using BugMortis.Domain.Entity;

namespace BugMortis.Domain
{
    public class ProjectService : IService<Domain.Entity.Project>
    {
        IRepository<Data.Entity.Project> _projectRepository;

        public ProjectService(IRepository<Data.Entity.Project> repository)
        {
            _projectRepository = repository;
        }

        public bool IsValid(Domain.Entity.Project project, out List<string> errorMessages)
        {
            var result = true;
            errorMessages = new List<string>();
            if (project == null)
            {
                result = false;
                errorMessages.Add("the project cannot be null");
            }
            else if (string.IsNullOrWhiteSpace(project.Name))
            {
                result = false;
                errorMessages.Add("the name of the project cannot be empty");
            }
            return result;
        }

        public Guid Insert(Domain.Entity.Project project)
        {
            var dataProject = MapToData(project);
            return _projectRepository.Insert(dataProject);
        }

        public Domain.Entity.Project GetById(Guid id)
        {
            var dataProject = _projectRepository.GetById(id);
            return MapToDomain(dataProject);
        }

        public IEnumerable<Domain.Entity.Project> GetAll()
        {
            var dataProjects = _projectRepository.GetAll();
            return dataProjects.Select(project => MapToDomain(project));
        }

        public void Delete(Guid id)
        {
            _projectRepository.Delete(id);
        }

        public void Update(Domain.Entity.Project project)
        {
            var dataProject = MapToData(project);
            _projectRepository.Update(dataProject);
        }

        public static Domain.Entity.Project MapToDomain(Data.Entity.Project project)
        {
            var result = new Domain.Entity.Project();
            result.IdProject = project.IdProject;
            result.IdCompany = project.IdCompany;
            result.Name = project.Name;
            result.Bugs = MapToDomainBugs(project.Bugs);
            result.Users = MapToDomainUsers(project.Users);
            return result;
        }

        public static Data.Entity.Project MapToData(Domain.Entity.Project project)
        {
            var result = new Data.Entity.Project();
            result.IdProject = project.IdProject;
            result.IdCompany = project.IdCompany;
            result.Name = project.Name;
            result.Bugs = MapToDataBugs(project.Bugs).ToList();
            result.Users = MapToDataUsers(project.Users).ToList();
            return result;
        }

        private static IEnumerable<Domain.Entity.Bug> MapToDomainBugs(IEnumerable<Data.Entity.Bug> bugs)
        {
            foreach(var bug in bugs)
            {
                yield return BugService.MapToDomain(bug);
            }
        }

        private static IEnumerable<Data.Entity.Bug> MapToDataBugs(IEnumerable<Domain.Entity.Bug> bugs)
        {
            foreach (var bug in bugs)
            {
                yield return BugService.MapToData(bug);
            }
        }

        private static IEnumerable<Domain.Entity.User> MapToDomainUsers(IEnumerable<Data.Entity.User> users)
        {
            foreach (var user in users)
            {
                yield return UserService.MapToDomain(user);
            }
        }

        private static IEnumerable<Data.Entity.User> MapToDataUsers(IEnumerable<Domain.Entity.User> users)
        {
            foreach (var user in users)
            {
                yield return UserService.MapToData(user);
            }
        }
    }
}

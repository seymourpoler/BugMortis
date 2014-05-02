using System;
using System.Linq;
using System.Collections.Generic;
using BugMortis.Data.Entity;

namespace BugMortis.Data.SqlServer.Repositories
{
    public class ProjectRepository : BaseRepository, IRepository<Project>
    {
        private BugRepository _bugRepository;
        private UserRepository _userRepository;
        public ProjectRepository(DataBaseContext db)
            : base(db)
        {
            _bugRepository = new BugRepository(db);
            _userRepository = new UserRepository(db);
        }

        public IEnumerable<Project> GetAll()
        {
            return _db.Projects;
        }

        public Guid Insert(Project project)
        {
            _db.Projects.Add(project);
            _db.SaveChanges();
            return project.IdProject;
        }

        public Project GetById(Guid idProject)
        {
            return _db.Projects.Find(idProject);
        }

        public void Update(Project project)
        {
            var projectDb = _db.Projects.Find(project.IdProject);
            if (projectDb != null)
            {
                projectDb.Name = project.Name;
                _db.Entry<Project>(project).State = System.Data.EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void Delete(Guid idProject)
        {
            var project = GetById(idProject);
            if (project != null)
            {
                DeleteBugs(project.Bugs.ToList());
                DeleteUsers(project.Users.ToList());
                _db.Projects.Remove(project);
                _db.SaveChanges();
            }
        }

        private void DeleteUsers(List<User> users)
        {
            users.ForEach( user => _userRepository.Delete(user.IdUser));            
        }

        private void DeleteBugs(List<Bug> bugs)
        {
            bugs.ForEach( bug => _bugRepository.Delete(bug.IdBug));
        }
    }
}

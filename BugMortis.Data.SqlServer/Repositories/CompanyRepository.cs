using System;
using System.Linq;
using System.Collections.Generic;
using BugMortis.Data;
using BugMortis.Data.Entity;

namespace BugMortis.Data.SqlServer.Repositories
{
    public class CompanyRepository : BaseRepository, IRepository<Company>
    {
        private ProjectRepository _projectRepository;
        public CompanyRepository(DataBaseContext db)
            : base(db)
        {
            _projectRepository = new ProjectRepository(db);
        }

        public IEnumerable<Company> GetAll()
        {
            return _db.Companies;
        }

        public Guid Insert(Company company)
        {
            _db.Companies.Add(company);
            _db.SaveChanges();
            return company.IdCompany;
        }

        public Company GetById(Guid idCompany)
        {
            return _db.Companies.Find(idCompany);
        }

        public void Update(Company company)
        {
            _db.Entry<Company>(company).State = System.Data.EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(Guid idCompany)
        {
            var company = _db.Companies.Find(idCompany);
            if (company != null) 
            {
                DeleteProjects(company.Projects.ToList());
                _db.Companies.Remove(company);
                _db.SaveChanges();
            }
        }

        private void DeleteProjects(List<Project> projects)
        {
            projects.ForEach(project => _projectRepository.Delete(project.IdProject));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using BugMortis.Data;
using BugMortis.Data.Entity;
using BugMortis.Data.SqlServer.Repositories;
using BugMortis.Domain.Entity;

namespace BugMortis.Domain
{
    public class CompanyService : IService<Domain.Entity.Company>
    {
        private IRepository<Data.Entity.Company> _companyRepository;

        public CompanyService(IRepository<Data.Entity.Company> companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public bool IsValid(Domain.Entity.Company company, out List<string> errorMessages) 
        {
            var result = true;
            errorMessages = new List<string>();
            if (company == null) 
            {
                result = false;
                errorMessages.Add("the company cannot be null");
            }
           else if(string.IsNullOrWhiteSpace(company.Name))
            {
                result = false;
                errorMessages.Add("the field name is required");
            }
            return result;
        }

        public IEnumerable<Domain.Entity.Company> GetAll()
        {
            var dataCompanies = _companyRepository.GetAll();
            return dataCompanies.Select(company => MapToDomain(company));
        }

        public Guid Insert(Domain.Entity.Company company)
        {
            var dataCompany = MapToData(company);
            return _companyRepository.Insert(dataCompany);
        }

        public void Delete(Guid idCompany)
        {
            _companyRepository.Delete(idCompany);
        }

        public Domain.Entity.Company GetById(Guid idCompany)
        {
            var dataCompany = _companyRepository.GetById(idCompany);
            return MapToDomain(dataCompany);
        }

        public void Update(Domain.Entity.Company company)
        {
            var dataCompany = MapToData(company);
            _companyRepository.Update(dataCompany);
        }

        internal static  Data.Entity.Company MapToData(Domain.Entity.Company company)
        {
            var dataCompany = new Data.Entity.Company();
            dataCompany.Name = company.Name;
            return dataCompany;
        }

        internal static Domain.Entity.Company MapToDomain(Data.Entity.Company company)
        {
            var domainCompany = new Domain.Entity.Company();
            domainCompany.Name = company.Name;
            return domainCompany;
        }
    }
}

using System;
using System.Collections.Generic;

namespace BugMortis.Data.Entity
{
    public class Company
    {
        public Guid IdCompany { get; set; }
        public string Name { get; set; }
        public ICollection<Project> Projects { get; set; }

        public Company()
        {
            Name = string.Empty;
            Projects = new List<Project>();
        }
    }
}

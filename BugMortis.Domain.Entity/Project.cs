using System;
using System.Collections.Generic;

namespace BugMortis.Domain.Entity
{
    public class Project
    {
        public Guid IdProject { get; set; }
        public string Name { get; set; }
        public Guid? IdCompany { get; set; }
        public IEnumerable<Bug> Bugs { get; set; }
        public IEnumerable<User> Users { get; set; }

        public Project()
        {
            IdCompany = null;
            Name = string.Empty;
            Bugs = new List<Bug>();
            Users = new List<User>();
        }
    }
}

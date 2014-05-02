using System;
using System.Collections.Generic;

namespace BugMortis.Data.Entity
{
    public class Project
    {
        public Guid IdProject { get; set; }
        public string Name { get; set; }
        public Guid? IdCompany { get; set; }
        public virtual Company Company { get; set; }
        public ICollection<Bug> Bugs { get; set; }
        public ICollection<User> Users { get; set; }

        public Project()
        {
            Name = string.Empty;
            Bugs = new List<Bug>();
            Users = new List<User>();
        }
    }
}
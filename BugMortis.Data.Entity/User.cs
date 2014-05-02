using System;
using System.Collections.Generic;

namespace BugMortis.Data.Entity
{
    public class User
    {
        public Guid IdUser { get; set; }
        public string Name { get; set; }
        public ICollection<Project> Projects { get; set; }

        public User()
        {
            Name = string.Empty;
            Projects = new List<Project>();
        }
    }
}

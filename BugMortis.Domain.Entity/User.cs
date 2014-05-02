using System;
using System.Collections.Generic;

namespace BugMortis.Domain.Entity
{
    public class User
    {
        public Guid IdUser { get; set; }
        public string Name { get; set; }

        public User()
        {
            Name = string.Empty;
        }
    }
}

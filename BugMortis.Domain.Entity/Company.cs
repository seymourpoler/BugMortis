using System;
using System.Collections.Generic;

namespace BugMortis.Domain.Entity
{
    public class Company
    {
        public Guid IdCompany { get; set; }
        public string Name { get; set; }

        public Company()
        {
            Name = string.Empty;
        }
    }
}

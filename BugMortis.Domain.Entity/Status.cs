using System;

namespace BugMortis.Domain.Entity
{
    public class Status
    {
        public Guid IdStatus { get; set; }
        public string Name { get; set; }

        public Status()
        {
            IdStatus = new Guid("00000000-0000-0000-0000-000000000001");
            Name = "Stopped";
        }
    }
}

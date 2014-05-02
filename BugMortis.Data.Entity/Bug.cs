using System;
using System.Collections.Generic;

namespace BugMortis.Data.Entity
{
    public class Bug
    {
        public Guid IdBug { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Days { get; set; }

        public Guid? IdPriority { get; set; }
        public virtual Priority Priority { get; set; }
        public Guid? IdStatus { get; set; }
        public virtual Status Status { get; set; }
        public Guid? IdProject { get; set; }
        public virtual Project Project { get; set; }
        public Guid? IdUser { get; set; }
        public virtual User User { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        
        public Bug()
        {
            Name = string.Empty;
            Description = string.Empty;
            Priority = null;
            Days = 0;
            IdStatus = null;
            IdProject = null;
            IdUser = null;
            Attachments = new List<Attachment>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BugMortis.Domain.Entity
{
    public class Bug
    {
        public Guid IdBug { get; set; }
        [Required]
        public string Name { get;set; }
        public string Description { get; set; }
        public int Days { get; set; }
        [Required]
        public Guid? IdPriority { get; set; }
        [Required]
        public Guid? IdStatus { get; set; }
        [Required]
        public Guid? IdProject { get; set; }
        public Guid? IdUser { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }

        public Bug()
        {
            Name = string.Empty;
            Description = string.Empty;
            IdPriority = null;
            Days = 0;
            IdStatus = null;
            IdProject = null;
            IdUser = null;
            Attachments = new List<Attachment>();
        }
    }
}

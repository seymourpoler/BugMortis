using System;
using System.ComponentModel.DataAnnotations;

namespace BugMortis.Domain.Entity
{
    public class Attachment
    {
        public Guid IdAttachment { get; set; }
        [Required]
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        [Required]
        public Guid IdBug { get; set; }

        public Attachment()
        {
            Name = string.Empty;
            ContentType = string.Empty;
        }
    }
}

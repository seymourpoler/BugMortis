using System;

namespace BugMortis.Data.Entity
{
    public class Attachment
    {
        public Guid IdAttachment{ get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public Guid IdBug { get; set; }
        public virtual Bug Bug { get; set; }

        public Attachment()
        {
            Name = string.Empty;
            ContentType = string.Empty;
        }
    }
}

using BugMortis.Data.Entity;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BugMortis.Data.SqlServer.Repositories
{
    public class AttachmentRepository : BaseRepository
    {
        public AttachmentRepository(DataBaseContext context)
            : base(context)
        { }

        public Guid Insert(Attachment attach)
        {
            _db.Attachments.Add(attach);
            return attach.IdAttachment;
        }

        public Attachment GetById(Guid idAttachment)
        {
            return _db.Attachments.Find(idAttachment);
        }

        public void Delete(Guid idAttachment)
        {
            var attachment = GetById(idAttachment);
            if(attachment != null)
            {
                _db.Attachments.Remove(attachment);
            }
        }

        public IEnumerable<Attachment> GetAll()
        {
            return _db.Attachments.AsEnumerable();
        }
    }
}

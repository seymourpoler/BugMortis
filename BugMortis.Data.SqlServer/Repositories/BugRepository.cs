using System;
using System.Linq;
using System.Collections.Generic;
using BugMortis.Data.Entity;

namespace BugMortis.Data.SqlServer.Repositories
{
    public class BugRepository : BaseRepository, IRepository<Bug>
    {
        private AttachmentRepository _attachmentRepository;
        public BugRepository(DataBaseContext context)
            : base(context)
        {
            _attachmentRepository = new AttachmentRepository(context);
        }

        public IEnumerable<Bug> GetAll()
        {
            return _db.Bugs;
        }

        public Guid Insert(Bug bug)
        {
            _db.Bugs.Add(bug);
            _db.SaveChanges();

            return bug.IdBug;
        }

        public void Update(Bug bug)
        {
            var currentBug = _db.Bugs.Find(bug.IdBug);
            if (currentBug != null)
            {
                currentBug.Days = bug.Days;
                currentBug.Name = bug.Name;
                currentBug.IdPriority = bug.IdPriority;
                currentBug.IdStatus = bug.IdStatus;
                AttachmentsManager(bug.IdBug, bug.Attachments.ToList());
                _db.Entry<Bug>(currentBug).State = System.Data.EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public Bug GetById(Guid idTask)
        {
            return _db.Bugs.Find(idTask);
        }

        public void Delete(Guid idTask)
        {
            var bug = _db.Bugs.Find(idTask);
            _db.Bugs.Remove(bug);
            _db.SaveChanges();
        }

        private void AttachmentsManager(Guid idBug, List<Attachment> attachments)
        {
            var dbAttachments = _attachmentRepository.GetAll().Where(x => x.IdBug == idBug).ToList();
            var attachmentsToAdd = GetAttachmentsToAdd(attachments, dbAttachments);
            var attachmentsToRemove = GetAttachmentsToRemove(attachments, dbAttachments);
            RemoveAttachents(attachmentsToRemove);
        }

        private IEnumerable<Attachment> GetAttachmentsToAdd(List<Attachment> attachments, List<Attachment> dbAttachments) 
        {
            foreach (var attachment in attachments)
            {
                if (!dbAttachments.Exists(x => x.IdAttachment == attachment.IdAttachment))
                {
                    yield return attachment;
                }
            }
            
        }
        
        private IEnumerable<Attachment> GetAttachmentsToRemove(List<Attachment> attachments, List<Attachment> dbAttachments) 
        {
            foreach (var attachment in dbAttachments)
            {
                if (!attachments.Exists(x => x.IdAttachment == attachment.IdAttachment))
                {
                    yield return attachment;
                }
            }
        }

        private void RemoveAttachents(IEnumerable<Attachment> attachments)
        {
            foreach (var attachment in attachments)
            {
                _attachmentRepository.Delete(attachment.IdAttachment);
            }
        }
    }
}
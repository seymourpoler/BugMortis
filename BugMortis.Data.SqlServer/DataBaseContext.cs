using System;
using System.Data.Entity;
using BugMortis.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugMortis.Data.SqlServer
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base() { }
        public DataBaseContext(string connectionString) : base(connectionString) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<Status> Status{ get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Priority> Priorities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            SetUpCompanies(modelBuilder);
            SetUpUsers(modelBuilder);
            SetUpBugs(modelBuilder);
            SetUpAttachments(modelBuilder);
            SetUpStatus(modelBuilder);
            SetUpPriority(modelBuilder);
            SetUpProjects(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetUpCompanies(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<Company>().HasKey(com => com.IdCompany);
            modelBuilder.Entity<Company>().Property(com => com.IdCompany).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Company>().Property(com => com.Name).IsRequired();
            modelBuilder.Entity<Company>().Property(com => com.Name).HasMaxLength(20);
        }

        private void SetUpUsers(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(usr => usr.IdUser);
            modelBuilder.Entity<User>().Property(usr => usr.IdUser).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<User>().Property(usr => usr.Name).IsRequired();
            modelBuilder.Entity<User>().Property(usr => usr.Name).HasMaxLength(50);
        }

        private void SetUpBugs(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bug>().ToTable("Bugs");
            modelBuilder.Entity<Bug>().HasKey(bg => bg.IdBug);
            modelBuilder.Entity<Bug>().Property(bg => bg.IdBug).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Bug>().Property(bg => bg.Name).IsRequired();
            modelBuilder.Entity<Bug>().Property(bg => bg.Name).HasMaxLength(20);
            modelBuilder.Entity<Bug>().Property(bg => bg.Description).HasMaxLength(100);
            modelBuilder.Entity<Bug>().Property(bg => bg.IdProject).IsRequired();
            modelBuilder.Entity<Bug>().Property(bg => bg.IdStatus).IsRequired();
            modelBuilder.Entity<Bug>().Property(bg => bg.IdPriority).IsRequired();
        }

        private void SetUpAttachments(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attachment>().ToTable("Attachments");
            modelBuilder.Entity<Attachment>().HasKey(at => at.IdAttachment);
            modelBuilder.Entity<Attachment>().Property(at => at.IdAttachment).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Attachment>().Property(at => at.Name).IsRequired();
            modelBuilder.Entity<Attachment>().Property(at => at.Name).HasMaxLength(20);
            modelBuilder.Entity<Attachment>().Property(at => at.IdBug).IsRequired();
        }

        private void SetUpStatus(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().ToTable("Status");
            modelBuilder.Entity<Status>().HasKey(st => st.IdStatus);
            modelBuilder.Entity<Status>().Property(st => st.IdStatus).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Status>().Property(st => st.Name).IsRequired();
            modelBuilder.Entity<Status>().Property(st => st.Name).HasMaxLength(50);
        }

        private void SetUpPriority(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Priority>().ToTable("Priorities");
            modelBuilder.Entity<Priority>().HasKey(p => p.IdPriority);
            modelBuilder.Entity<Priority>().Property(p => p.IdPriority).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Priority>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Priority>().Property(p => p.Name).HasMaxLength(50);
        }

        private void SetUpProjects(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable("Projects");
            modelBuilder.Entity<Project>().HasKey(pj => pj.IdProject);
            modelBuilder.Entity<Project>().Property(pj => pj.IdProject).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Project>().Property(pj => pj.Name).IsRequired();
            modelBuilder.Entity<Project>().Property(pj => pj.Name).HasMaxLength(50);
        }
    }
}

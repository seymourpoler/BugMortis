using System;
using System.Data.Entity;
using BugMortis.Data.Entity;
using BugMortis.Data.SqlServer;

namespace BugMortis.Data.SqlServer
{
    public class DataBaseContextSeedData : DropCreateDatabaseIfModelChanges<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            LoadStatus(context);
            LoadPriority(context);
        }

        private void LoadStatus(DataBaseContext context)
        {
            context.Status.Add(new Status { IdStatus = new Guid("00000000-0000-0000-0000-000000000001"), Name = "Stopped" });
            context.Status.Add(new Status { IdStatus = new Guid("00000000-0000-0000-0000-000000000002"), Name = "Executing" });
            context.Status.Add(new Status { IdStatus = new Guid("00000000-0000-0000-0000-000000000003"), Name = "Done" });
            context.SaveChanges();
        }

        private void LoadPriority(DataBaseContext context)
        {
            context.Priorities.Add(new Priority{ IdPriority = new Guid("00000000-0000-0000-0000-000000000001"), Name = "High" });
            context.Priorities.Add(new Priority { IdPriority = new Guid("00000000-0000-0000-0000-000000000002"), Name = "Medium" });
            context.Priorities.Add(new Priority { IdPriority = new Guid("00000000-0000-0000-0000-000000000003"), Name = "Low" });
            context.SaveChanges();
        }
    }
}

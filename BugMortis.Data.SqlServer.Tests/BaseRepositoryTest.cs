using System;
using System.Data.Entity;
using BugMortis.Data.SqlServer;

namespace BugMortis.Data.SqlServer.Tests
{
    public class BaseRepositoryTest
    {
        protected DataBaseContext _db;
        private string _connectionString = @"Data Source=PORTATIL\SQLEXPRESS;Initial Catalog=Mantis;Integrated Security=True";

        public BaseRepositoryTest()
        {
            Database.SetInitializer(new DataBaseContextSeedData());
            _db = new DataBaseContext(_connectionString);
        }
    }
}

using System;

namespace BugMortis.Data.SqlServer.Repositories
{
    public class BaseRepository
    {
        //protected string _connectionString = @"Data Source=PORTATIL\SQLEXPRESS;Initial Catalog=Tasks;Integrated Security=True";
         protected  DataBaseContext _db;
        public BaseRepository( DataBaseContext db)
        {
             _db = db;
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using BugMortis.Data.Entity;

namespace BugMortis.Data.SqlServer.Repositories
{
    public class UserRepository : BaseRepository, IRepository<User>
    {
        public UserRepository(DataBaseContext db) : base(db)
        {
        }

        public IEnumerable<User> GetAll()
        {
            return _db.Users;
        }

        public Guid Insert(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
            return user.IdUser;
        }

        public User GetById(Guid idUser)
        {
            return _db.Users.Find(idUser);
        }

        public void Update(User user)
        {
            var userDb = _db.Users.Find(user.IdUser);
            if (userDb != null)
            {
                userDb.Name = user.Name;
                _db.SaveChanges();
            }
        }

        public void Delete(Guid idUser)
        {
            var user = GetById(idUser);
            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
            }
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using BugMortis.Data;
using BugMortis.Data.Entity;
using BugMortis.Domain.Entity;

namespace BugMortis.Domain
{
    public class UserService : IService<Domain.Entity.User>
    {
        private IRepository<Data.Entity.User> _userRepository;

        public UserService(IRepository<Data.Entity.User> userRepository)
        {
            _userRepository = userRepository;
        }

        public bool IsValid(Domain.Entity.User user, out List<string> errorMessages)
        {
            var result = true;
            errorMessages = new List<string>();
            if(user == null)
            {
                result = false;
                errorMessages.Add("user cannot be null");
            }
            else if (string.IsNullOrWhiteSpace(user.Name))
            {
                result = false;
                errorMessages.Add("the name of the user cannot be empty");
            }
            return result;
        }

        public Guid Insert(Domain.Entity.User user)
        {
            var dataUser = MapToData(user);
            return _userRepository.Insert(dataUser);
        }

        public Domain.Entity.User GetById(Guid IdUser)
        {
            var dataUser = _userRepository.GetById(IdUser);
            return MapToDomain(dataUser);
        }

        public IEnumerable<Domain.Entity.User> GetAll()
        {
            var dataUsers = _userRepository.GetAll();
            return dataUsers.Select(user => MapToDomain(user));
        }

        public void Update(Domain.Entity.User user)
        {
            var dataUser = MapToData(user);
            _userRepository.Update(dataUser);
        }

        public void Delete(Guid idUser)
        {
            _userRepository.Delete(idUser);
        }

        public static Data.Entity.User MapToData(Domain.Entity.User user)
        {
            var result = new Data.Entity.User();
            result.IdUser = user.IdUser;
            result.Name = user.Name;
            return result;
        }

        public static Domain.Entity.User MapToDomain(Data.Entity.User user)
        {
            var result = new Domain.Entity.User();
            result.IdUser = user.IdUser;
            result.Name = user.Name;
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain;
using Domain.Exceptions;

namespace BusinessLogic
{
    public class UserService
    {
        private readonly MemoryDatabase _memoryDatabase;

        public UserService(MemoryDatabase memoryDatabase)
        {
            this._memoryDatabase = memoryDatabase;
        }

        public void Add(User u)
        {
            if (_memoryDatabase.Users.Any(x => x.Email == u.Email))
            {
                throw new UserAlreadyExistsException();
            }
            try
            {
              _memoryDatabase.Users.Add(u);
              Workspace defaultWorkspace = new Workspace(u, $"Personal {u.Name} {u.LastName}");
              u.WorkspaceList.Add(defaultWorkspace);
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public User Get(string email)
        {
            return _memoryDatabase.Users.FirstOrDefault(x => x.Email == email);
        }

        public void UpdateEmail(User user, string newEmail)
        {
            User alreadyExists = _memoryDatabase.Users.FirstOrDefault(x => x.Email == newEmail);

            if (alreadyExists != null)
            {
                throw new UserAlreadyExistsException();
            }

            this.Get(user.Email).Email = newEmail;
        }

        public void DeleteUser(User user)
        {
            _memoryDatabase.Users.Remove(user);
        }

        public bool Login(string email, string password)
        {
            User validUser = _memoryDatabase.Users.FirstOrDefault(x => x.Email == email && x.Password == password);

            if (validUser != null)
            {
                return true;
            }
            else
            {
                throw new InvalidUserException();
            }


        }
    }
}
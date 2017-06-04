using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeTester.IDAL;
using KnowledgeTester.Model;

namespace KnowledgeTester.DAL
{
    class UsersRepository : Repository, IUsersRepository
    {
        public User GetUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public User GetUserByLoginPassword(string login, string password)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public void SaveUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}

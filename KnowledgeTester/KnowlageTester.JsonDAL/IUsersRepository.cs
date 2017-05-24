using System;
using System.Collections.Generic;
using KnowledgeTester.Model;

namespace KnowlageTester.JsonDAL
{
    public interface IUsersRepository
    {
        IList<User> GetUsers();
        User GetUser(Guid id);
        void SaveUser(User user);
    }
}

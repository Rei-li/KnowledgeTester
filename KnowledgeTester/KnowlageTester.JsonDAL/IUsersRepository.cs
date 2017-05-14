using System;
using System.Collections.Generic;
using KnowledgeTester.Model;

namespace KnowlageTester.JsonDAL
{
    interface IUsersRepository
    {
        IList<User> GetUsers();
        User GetUser(Guid id);
        void SaveUser(User user);
    }
}

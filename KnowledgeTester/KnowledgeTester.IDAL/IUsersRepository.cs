using System;
using System.Collections.Generic;
using KnowledgeTester.Model;

namespace KnowledgeTester.IDAL
{
    public interface IUsersRepository
    {
        void SaveUser(User user);
        User GetUserByLoginPassword(string login, string password);
    }
}

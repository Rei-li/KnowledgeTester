using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using KnowledgeTester.Model;

namespace KnowlageTester.JsonDAL
{
    public class UsersRepository : Repository<User>,  IUsersRepository
    {
        //private readonly Repository<User> _repo = new Repository<User>(ConfigurationManager.AppSettings["UsersCollectionFolder"]);
        public override string CollectionPath => ConfigurationManager.AppSettings["UsersCollectionFolder"];

        public User GetUser(Guid id)
        {
            return GetItem(id);
        }

        public User GetUserByLogin(string login)
        {
            var users = GetUsers().Where(s => s.Login.Equals(login)).ToList();
            if (users.Any())
            {
                return users.First();
            }
            return null;
        }

        public IList<User> GetUsers()
        {
            return GetAllItems();
        }

        public void SaveUser(User user)
        {
            SaveItem(user);
        }
    }
}
using System;
using System.Collections.Generic;
using KnowlageTester.JsonDAL;
using KnowledgeTester.Model;

namespace KnowledgeTester.BLL
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepo;

        public UserService(IUsersRepository usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public void SaveUser()
        {
            _usersRepo.SaveUser(new User {Id = Guid.NewGuid(), Login = "TestUser", Password = "123456", LastName = "Test", Name = "User", TakenTests = new List<UserTest>()});
        }
    }
}
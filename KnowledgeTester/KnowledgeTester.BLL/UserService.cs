using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using KnowlageTester.JsonDAL;
using KnowledgeTester.Model;

namespace KnowledgeTester.BLL
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepo;
        private User _currentUser;

        public UserService(IUsersRepository usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public void SaveUser(string name, string lastName, string login, string password)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, password);
                _usersRepo.SaveUser(new User
                {
                    Id = Guid.NewGuid(),
                    Login = login,
                    Password = hash,
                    LastName = lastName,
                    Name = name,
                    TakenTests = new List<UserTest>()
                });
            }


        }

        public User Login(string login, string password)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                _currentUser = _usersRepo.GetUserByLoginPassword(login, GetMd5Hash(md5Hash, password));
            }
            return GetCurrentUser();
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public User Logout()
        {
            _currentUser = null;
            return GetCurrentUser();
        }


        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
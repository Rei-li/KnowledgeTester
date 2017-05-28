using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeTester.Model;

namespace KnowledgeTester.BLL
{
    public interface IUserService
    {
        void SaveUser(string name, string lastName, string login, string password);

        User Login(string login, string password);
        User Logout();
        User GetCurrentUser();

    }
}

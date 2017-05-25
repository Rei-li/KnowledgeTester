using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeTester.BLL
{
    public interface IUserService
    {
        void SaveUser(string name, string lastName, string login, string password);
    }
}

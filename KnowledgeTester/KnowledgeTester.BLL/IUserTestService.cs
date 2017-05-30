using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeTester.Model;

namespace KnowledgeTester.BLL
{
    public interface IUserTestService
    {
        void SaveUserTest(Test test, List<UserAnswer> answers);
    }
}

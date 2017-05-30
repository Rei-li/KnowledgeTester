using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeTester.Model;

namespace KnowlageTester.JsonDAL
{
    public interface IUserTestsRepository
    {
        IList<UserTest> GetTests();
        UserTest GetTest(Guid id);
        void SaveTest(UserTest test);
    }
}

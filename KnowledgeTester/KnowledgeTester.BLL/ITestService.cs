using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeTester.Model;

namespace KnowledgeTester.BLL
{
    public interface ITestService
    {
        void SaveTest(string theme, string author, IList<Question> questions);
        IList<Test> GetTests();
        Test GetTest(Guid id);
    }
}

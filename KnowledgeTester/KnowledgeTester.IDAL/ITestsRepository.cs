using System;
using System.Collections.Generic;
using KnowledgeTester.Model;

namespace KnowledgeTester.IDAL
{
    public interface ITestsRepository
    {
        IList<Test> GetTests();
        Test GetTest(Guid id);
        void SaveTest(Test test);
        void DeleteItem(Guid id);
    }
}

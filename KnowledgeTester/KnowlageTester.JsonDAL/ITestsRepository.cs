using System;
using System.Collections.Generic;
using KnowledgeTester.Model;

namespace KnowlageTester.JsonDAL
{
    public interface ITestsRepository
    {
        IList<Test> GetTests();
        Test GetTest(Guid id);
        void SaveTest(Test test);
    }
}

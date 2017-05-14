using System;
using System.Collections.Generic;
using System.Configuration;
using KnowledgeTester.Model;

namespace KnowlageTester.JsonDAL
{
    public class TestsRepository: ITestsRepository
    {
         private readonly Repository<Test> _repo = new Repository<Test>(ConfigurationManager.AppSettings["TestsCollectionFolder"]);

        public IList<Test> GetTests()
        {
            return _repo.GetAllItems();
        }

        public Test GetTest(Guid id)
        {
            return _repo.GetItem(id);
        }

        public void SaveTest(Test test)
        {
            _repo.SaveItem(test);
        }

    }
}
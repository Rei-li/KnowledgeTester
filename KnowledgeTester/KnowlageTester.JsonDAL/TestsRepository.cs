using System;
using System.Collections.Generic;
using System.Configuration;
using KnowledgeTester.IDAL;
using KnowledgeTester.Model;

namespace KnowlageTester.JsonDAL
{
    public class TestsRepository : Repository<Test>, ITestsRepository
    {
        //private readonly Repository<Test> _repo = new Repository<Test>(ConfigurationManager.AppSettings["TestsCollectionFolder"]);

        public override string CollectionPath => ConfigurationManager.AppSettings["TestsCollectionFolder"];

        public IList<Test> GetTests()
        {
            return GetAllItems();
        }

        public Test GetTest(Guid id)
        {
            return GetItem(id);
        }

        public void SaveTest(Test test)
        {
            SaveItem(test);
        }

    }
}
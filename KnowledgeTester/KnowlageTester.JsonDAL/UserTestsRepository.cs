using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeTester.Model;

namespace KnowlageTester.JsonDAL
{
    class UserTestsRepository: Repository<UserTest>, IUserTestsRepository
    {
        public override string CollectionPath => ConfigurationManager.AppSettings["UserTestsCollectionFolder"];

        public IList<UserTest> GetTests()
        {
            return GetAllItems();
        }

        public UserTest GetTest(Guid id)
        {
            return GetItem(id);
        }

        public void SaveTest(UserTest test)
        {
            SaveItem(test);
        }
    }
}

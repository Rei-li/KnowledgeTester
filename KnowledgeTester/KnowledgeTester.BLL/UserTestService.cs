using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowlageTester.JsonDAL;
using KnowledgeTester.Model;

namespace KnowledgeTester.BLL
{
    class UserTestService : IUserTestService
    {
        private readonly IUserTestsRepository _userTestsRepo;

        public UserTestService(IUserTestsRepository userTestsRepo)
        {
            _userTestsRepo = userTestsRepo;
        }

        public void SaveUserTest(Test test, List<UserAnswer> answers)
        {
            _userTestsRepo.SaveTest(new UserTest
            {
               Id = Guid.NewGuid(),
                Test = test,
                Answers = answers,
                Score = 0

            });
        }
    }
}

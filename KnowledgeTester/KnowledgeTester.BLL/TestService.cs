﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowlageTester.JsonDAL;
using KnowledgeTester.DAL;
using KnowledgeTester.Model;
using KnowledgeTester.IDAL;

namespace KnowledgeTester.BLL
{
    class TestService : ITestService
    {
        private readonly ITestsRepository _testsRepo;

        public TestService(ITestsRepository testsRepo)
        {
            _testsRepo = testsRepo;
        }

        public void SaveTest(string theme, string author, IList<Question> questions)
        {
            _testsRepo.SaveTest(new Test
            {
                Version = 1,
                Id = Guid.NewGuid(),
                Theme = theme,
                Author = author,
                Question = questions.ToList()
            });
        }

        public void SaveTest(Test test)
        {
            _testsRepo.SaveTest(test);
        }

        public IList<Test> GetTests()
        {
            return _testsRepo.GetTests();
        }

        public Test GetTest(Guid id)
        {
            return _testsRepo.GetTest(id);
        }

        public void DeleteTest(Test test)
        {
            _testsRepo.DeleteItem(test.Id);
        }
    }
}

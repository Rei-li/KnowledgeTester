using System;
using System.Collections.Generic;

namespace KnowledgeTester.Model
{
    public class UserTest: BaseItem
    {
        public Test Test { set; get; }
        public List<UserAnswer> Answers { set; get; }
        public decimal Score { set; get; }
        public DateTime Time { set; get; }
    }
}
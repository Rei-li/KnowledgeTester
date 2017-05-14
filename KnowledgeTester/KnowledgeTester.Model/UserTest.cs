using System.Collections.Generic;

namespace KnowledgeTester.Model
{
    public class UserTest: BaseItem
    {
        public Test Test { set; get; }
        public List<UserAnswer> Answers { set; get; }
        public int Score { set; get; }
    }
}
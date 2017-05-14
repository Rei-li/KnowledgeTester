using System.Collections.Generic;

namespace KnowledgeTester.Model
{
    public class User : BaseItem
    {
        public string Name { set; get; }
        public string LastName { set; get; }
        public string Login { set; get; }
        public string Password { set; get; }
        public List<UserTest> TakenTests{ set; get; }
    }
}
using System.Collections.Generic;

namespace KnowledgeTester.Model
{
    public class Question : BaseItem
    {
        public int Number { set; get; }
        public string Task { set; get; }
        public string Notes { set; get; }
        public List<Answer> Answers { set; get; }
    }
}
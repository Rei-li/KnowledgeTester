using System;
using System.Collections.Generic;
using KnowledgeTester.Model;

namespace KnowledgeTester.Model
{
    public class Test
    {
        public Guid Id { set; get; }
        public string Theme { set; get; }
        public string Author { set; get; }
        public List<Question> Question { set; get; }
    }
}
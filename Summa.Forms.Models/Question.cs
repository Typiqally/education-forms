using System;
using System.Collections.Generic;

namespace Summa.Forms.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public QuestionType Type { get; set; }
        public int Index { get; set; }
        public string Value { get; set; }
        public List<QuestionOption> Options { get; set; }
    }
}
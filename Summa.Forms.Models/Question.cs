using System;
using System.Collections.Generic;

namespace Summa.Forms.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        
        public QuestionType Type { get; set; }
        
        public List<QuestionOption> Options { get; set; }
    }
}
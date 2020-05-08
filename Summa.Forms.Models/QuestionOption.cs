using System;

namespace Summa.Forms.Models
{
    public class QuestionOption
    {
        public Guid Id { get; set; }
        
        public QuestionType Type { get; set; }
        
        public string Value { get; set; }
    }
}
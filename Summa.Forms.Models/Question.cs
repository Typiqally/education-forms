using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Summa.Forms.Models
{
    [Bind("Type,Index,Value")]
    public class Question
    {
        public Guid Id { get; set; }
        public QuestionType Type { get; set; }
        public int Index { get; set; }
        public string Value { get; set; }
        public List<QuestionOption> Options { get; set; }
    }
}
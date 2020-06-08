using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Summa.Forms.Models
{
    [Bind("Type,Index,Value")]
    public class Question
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid FormId { get; set; }

        [JsonIgnore]
        public Form Form { get; set; }

        public QuestionType Type { get; set; }
        public int Index { get; set; }
        public string Value { get; set; }
        public ICollection<QuestionOption> Options { get; set; }
    }
}
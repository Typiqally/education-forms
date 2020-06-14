using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Summa.Forms.Models
{
    [Bind("Index,Key,Value")]
    public class QuestionOption
    {
        public Guid Id { get; set; }
        [JsonIgnore]
        public Guid QuestionId { get; set; }
        [JsonIgnore]
        public Question Question { get; set; }
        public QuestionType Type { get; set; }
        public int Index { get; set; }
        public string Title { get; set; }
        public int Value { get; set; }
    }
}
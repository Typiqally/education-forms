using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Summa.Forms.Models
{
    [Bind("QuestionId,Value")]
    public class QuestionAnswer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        [JsonIgnore]
        public Question Question { get; set; }
        public string Value { get; set; }
    }
}
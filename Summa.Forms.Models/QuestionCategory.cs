using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Summa.Forms.Models
{
    [Bind("Value")]
    public class QuestionCategory
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public Guid FormId { get; set; }
        [JsonIgnore]
        public Form Form { get; set; }
        [JsonIgnore]
        public Question Question { get; set; }
    }
}
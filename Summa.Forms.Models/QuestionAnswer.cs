using System;
using System.ComponentModel.DataAnnotations.Schema;
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

        [JsonIgnore]
        [NotMapped]
        public Guid CategoryId { get; set; }

        [NotMapped]
        public QuestionCategory Category { get; set; }

        public int Value { get; set; }
    }
}
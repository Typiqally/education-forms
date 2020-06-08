using System;
using Microsoft.AspNetCore.Mvc;

namespace Summa.Forms.Models
{
    [Bind("QuestionId,Value")]
    public class QuestionAnswer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public Guid UserId { get; set; }
        public string Value { get; set; }
    }
}
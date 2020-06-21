using System;
using Microsoft.AspNetCore.Mvc;

namespace Summa.Forms.Models
{
    [Bind("Value")]
    public class QuestionCategory
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }
}
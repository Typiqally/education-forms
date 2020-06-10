using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Summa.Forms.Models
{
    public class FormResponse
    {
        public Guid Id { get; set; }
        [JsonIgnore]
        public Guid FormId { get; set; }
        [JsonIgnore]
        public Form Form { get; set; }
        public Guid UserId { get; set; }
        public ICollection<QuestionAnswer> Answers { get; set; }
    }
}
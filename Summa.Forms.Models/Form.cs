using System;
using System.Collections.Generic;

namespace Summa.Forms.Models
{
    public class Form
    {
        public Guid Id { get; set; }

        public Guid AuthorId { get; set; }

        public FormCategory Category { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime TimeCreated { get; set; }

        public List<Question> Questions { get; set; }
    }
}
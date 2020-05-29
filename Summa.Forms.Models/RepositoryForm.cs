using System;

namespace Summa.Forms.Models
{
    public class RepositoryForm
    {
        public Guid Id { get; set; }
        public Form Form { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
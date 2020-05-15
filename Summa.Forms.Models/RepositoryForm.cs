using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Summa.Forms.Models
{
    [Table("Repository")]
    public class RepositoryForm
    {
        public Guid Id { get; set; }

        public Form Form { get; set; }

        public DateTime PublishDate { get; set; }
    }
}
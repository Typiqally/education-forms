using Microsoft.EntityFrameworkCore;
using Summa.Forms.Models;

namespace Summa.Forms.WebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Form> Forms { get; set; }

        public DbSet<RepositoryForm> Repository { get; set; }
    }
}
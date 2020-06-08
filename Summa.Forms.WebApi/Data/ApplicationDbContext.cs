using Microsoft.EntityFrameworkCore;
using Summa.Forms.Models;

namespace Summa.Forms.WebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RepositoryForm>().ToTable("Repository");
            modelBuilder.Entity<FormCategory>().ToTable("FormCategory");

            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasMany(x => x.Questions)
                    .WithOne(x => x.Form)
                    .HasForeignKey(x => x.FormId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasMany(x => x.Options)
                    .WithOne(x => x.Question)
                    .HasForeignKey(x => x.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public DbSet<Form> Forms { get; set; }

        public DbSet<QuestionAnswer> Answers { get; set; }

        public DbSet<FormCategory> Categories { get; set; }

        public DbSet<RepositoryForm> Repository { get; set; }
    }
}
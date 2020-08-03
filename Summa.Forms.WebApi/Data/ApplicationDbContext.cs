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

            modelBuilder.Entity<Form>().ToTable("Form");
            modelBuilder.Entity<FormResponse>().ToTable("FormResponse");
            modelBuilder.Entity<FormCategory>().ToTable("FormCategory");
            modelBuilder.Entity<RepositoryForm>().ToTable("Repository");

            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasMany(x => x.Questions)
                    .WithOne(x => x.Form)
                    .HasForeignKey(x => x.FormId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(x => x.Categories)
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

        public DbSet<FormResponse> Responses { get; set; }

        public DbSet<FormCategory> Categories { get; set; }

        public DbSet<RepositoryForm> Repository { get; set; }
    }
}
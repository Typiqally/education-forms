using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Summa.Forms.Models;

namespace Summa.Forms.WebApp.Data
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
        }

        public DbSet<Form> Forms { get; set; }

        public DbSet<FormCategory> Categories { get; set; }
        
        public DbSet<RepositoryForm> Repository { get; set; }
    }
}
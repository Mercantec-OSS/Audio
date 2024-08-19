using Microsoft.EntityFrameworkCore;
using System;
using Models;

namespace DBAccess
{
    public class DBContext : DbContext
    {
        public DbSet<Audio> Audio { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Genre> Genre { get; set; }

        public DbSet<Models.Type> Type { get; set; }

        public DbSet<Loudness> Loudness { get; set; }

        public DbSet<Instrument> Instrument { get; set; }

        public DbSet<Mood> Mood { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=Audio;Trusted_Connection=Yes;TrustServerCertificate=True");
        }
    }
}

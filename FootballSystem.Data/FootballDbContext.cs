﻿namespace FootballSystem.Data
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure.Annotations;

    using Models;

    public class FootballDbContext : DbContext
    {
        public FootballDbContext()
            : base("FootballDatabase")
        {
            Database.SetInitializer<FootballDbContext>(new CreateDatabaseIfNotExists<FootballDbContext>());
        }

        public IDbSet<Player> Players { get; set; }

        public IDbSet<Team> Teams { get; set; }

        public IDbSet<Country> Countries { get; set; }

        public IDbSet<Championship> Championships { get; set; }

        public IDbSet<City> Cities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            OnCountryModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void OnCountryModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .Property(country => country.Name)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_Name") { IsUnique = true }));
        }
    }
}

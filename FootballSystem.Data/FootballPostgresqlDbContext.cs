﻿namespace FootballSystem.Data
{
    using System.Data.Entity;

    using Models;

    public class FootballPostgresqlDbContext : DbContext
    {
        public FootballPostgresqlDbContext()
            : base("PostgresFootball")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public IDbSet<Player> Players { get; set; }

        public IDbSet<Team> Teams { get; set; }

        public IDbSet<Country> Countries { get; set; }

        public IDbSet<Championship> Championships { get; set; }

        public IDbSet<City> Cities { get; set; }

        public IDbSet<Advertisements> Advertisements { get; set; }

        public IDbSet<Coach> Coaches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            ModelCreating.AllModels(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}

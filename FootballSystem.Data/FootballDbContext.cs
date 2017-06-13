namespace FootballSystem.Data
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

        public IDbSet<Coach> Coaches { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelCreating.AllModels(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}

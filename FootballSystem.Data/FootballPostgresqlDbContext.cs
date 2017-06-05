namespace FootballSystem.Data
{
    using Models;
    using System.Data.Entity;
   public class FootballPostgresqlDbContext : DbContext
    {

        public FootballPostgresqlDbContext()
            :base("PostgresFootball")
        {

        }

        public IDbSet<Player> Players { get; set; }

        public IDbSet<Team> Teams { get; set; }

        public IDbSet<Country> Countries { get; set; }

        public IDbSet<Championship> Championships { get; set; }

        public IDbSet<City> Cities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}

namespace FootballSystem.Data
{
    using FootballSystem.Models;
    using System.Data.Entity;
    public class FootballDbContext : DbContext
    {

        public FootballDbContext()
            :base("FootballDatabase")
        {
            Database.SetInitializer<FootballDbContext>(new CreateDatabaseIfNotExists<FootballDbContext>());
        }
        public IDbSet<Player> Players { get; set; }

        public IDbSet<Team> Teams { get; set; }

        public IDbSet<Country> Countries { get; set; }

        public IDbSet<Championship> Championships { get; set; }

        public IDbSet<City> Cities { get; set; }
    }
}

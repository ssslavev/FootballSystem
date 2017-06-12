namespace FootballSystem.Data
{
    using System.Data.Entity;

    using Models;

    using SQLite.CodeFirst;

    public class FootballSQLiteDbContext : DbContext
    {
        public FootballSQLiteDbContext()
            : base("SQLiteFootball")
        {
        }

        public IDbSet<Player> Players { get; set; }

        public IDbSet<Team> Teams { get; set; }

        public IDbSet<Country> Countries { get; set; }

        public IDbSet<Championship> Championships { get; set; }

        public IDbSet<City> Cities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<FootballSQLiteDbContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}

using FootballSystem.Models;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballSystem.Data
{
    public class FootballSQLiteDbContext: DbContext
    {
        public FootballSQLiteDbContext()
            :base("SQLiteFootball")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<FootballSQLiteDbContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);


        }


        

        public IDbSet<Player> Players { get; set; }

        public IDbSet<Team> Teams { get; set; }

        public IDbSet<Country> Countries { get; set; }

        public IDbSet<Championship> Championships { get; set; }

        public IDbSet<City> Cities { get; set; }
    }
}

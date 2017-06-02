namespace FootballSystem.ConsoleClient
{
    using Data.Migrations;
    using FootballSystem.Data;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;
    

    public class Startup
    {
        static void Main()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FootballDbContext, Configuration>());

            var db = new FootballDbContext();

            var city = new City
            {
                Name = "Sofia"
            };

            var player = new Player
            {
                FirstName = "Leo",
                LastName = "Messi",
               
               
            };

                db.Players.Add(player);
                db.Cities.Add(city);
                db.SaveChanges();
            
            Console.WriteLine(db.Cities.Count());
            Console.WriteLine(db.Players.Count());

        }
    }
}

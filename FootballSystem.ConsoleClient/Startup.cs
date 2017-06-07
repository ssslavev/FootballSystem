namespace FootballSystem.ConsoleClient
{
    using Data.Migrations;
    using Data;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.IO;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Excel;
    using System.Data;

    public class Startup
    {
        static void ImportCountriesFromJSONToSQLServer()
        {
            //import countries from json file
            var countriesToAdd = Directory.GetFiles(Directory.GetCurrentDirectory())
                    .Where(f => f.EndsWith("json"))
                    .Select(f => File.ReadAllText(f))
                    .SelectMany(str => JsonConvert.DeserializeObject<IEnumerable<ParseModel>>(str))
                    .ToList();

            var db = new FootballDbContext();
            var counter = 0;

            foreach (var country in countriesToAdd)
            {
                var countryToAdd = new Country
                {
                    Name = country.Name
                };

                db.Countries.Add(countryToAdd);
                counter++;

            }

            db.SaveChanges();

            Console.WriteLine($"{counter} countries was added!");
            Console.WriteLine(new String('*', 50));

        }

        static void ImportCitiesFromExcellToSQLServer()
        {
            var db = new FootballDbContext();

            //import cities from excell file
            FileStream stream = File.Open(Directory.GetCurrentDirectory() + "/cities.xlsx", FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);



            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();

            DataTable dt = result.Tables[0];

            var counter = 0;

            foreach (DataRow row in dt.Rows)
            {


                string countryName = row.Field<string>(1).Trim();

                var country = db.Countries.FirstOrDefault(co => co.Name == countryName);


                var cityToAdd = new City
                {
                    Name = row.Field<string>(0).Trim(),
                    Country = country
                };

                db.Cities.Add(cityToAdd);
                counter++;

            }

            db.SaveChanges();
            excelReader.Close();

            Console.WriteLine($"{counter} cities was added!");
            Console.WriteLine(new String('*', 50));

        }

        static void AddPlayer()
        {
            var db = new FootballDbContext();

            Console.Write("First name: ");
            var firstName = Console.ReadLine();
            Console.Write("Last name: ");
            var lastName = Console.ReadLine();
            Console.Write("Age: ");
            var age = int.Parse(Console.ReadLine());
            Console.Write("Salary: ");
            var salary = int.Parse(Console.ReadLine());
            Console.Write("Player country: ");
            var playerCountry = Console.ReadLine();
            Console.Write("Team name: ");
            var teamName = Console.ReadLine();
            Console.Write("Team manager: ");
            var teamManager = Console.ReadLine();
            Console.Write("Team stadium: ");
            var teamStadium = Console.ReadLine();
            Console.Write("Team country: ");
            var teamCountry = Console.ReadLine();
            Console.Write("Team Championship: ");
            var teamChampionship = Console.ReadLine();
            Console.Write("Team City: ");
            var teamCity = Console.ReadLine();


            var player = new Player
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Salary = salary,
                Country = new Country { Name = playerCountry },
                Team = new Team
                {
                    Name = teamName,
                    Manager = teamManager,
                    Stadium = teamStadium,
                    Country = new Country { Name = teamCountry },
                    Championship = new Championship { Name = teamChampionship },
                    City = new City { Name = teamCity }
                },

            };

            db.Players.Add(player);
            db.SaveChanges();
            Console.WriteLine($"Player {firstName} {lastName} was added!");
            Console.WriteLine(new String('*', 50));
        }

        static void RemovePlayer()
        {
            var db = new FootballDbContext();

            Console.Write("Player name: ");
            var playerName = Console.ReadLine();

            var row = db.Database.ExecuteSqlCommand($"DELETE FROM Players WHERE FirstName = '{playerName}'");

            if (row==1)
            {
                Console.WriteLine($"The player {playerName} was deleted!");
                Console.WriteLine(new String('*', 50));
            }
            else
            {
                Console.WriteLine($"No player with that name!");
                Console.WriteLine(new String('*', 50));
            }

            
            
        }

        static void Main()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FootballDbContext, Configuration>());

            var db = new FootballDbContext();

            var postgresqlDb = new FootballPostgresqlDbContext();

            var sqliteDb = new FootballSQLiteDbContext();


            Console.WriteLine("1. Import countries from json file to sql server");
            Console.WriteLine("2. Import cities from excell to sql server");
            Console.WriteLine("3. Add new player");
            Console.WriteLine("4. Remove player by player first name");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Enter the number of the command you want!");
                Console.Write("number:  ");

                var commandNumber = int.Parse(Console.ReadLine());

                if (commandNumber == 1)
                {
                    ImportCountriesFromJSONToSQLServer();

                }
                else if (commandNumber == 2)
                {
                    ImportCitiesFromExcellToSQLServer();
                }

                else if (commandNumber == 3)
                {
                    AddPlayer();
                }

                else if (commandNumber == 4)
                {
                    RemovePlayer();
                }


            }
        }
    }
}









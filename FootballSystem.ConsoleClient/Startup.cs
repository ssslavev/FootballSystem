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
    using System.Xml.Linq;

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

            if (row == 1)
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

        static void FindPlayer()
        {
            var db = new FootballDbContext();

            Console.Write("Player name: ");
            var playerName = Console.ReadLine();

            var player = db.Players.Where(pl => pl.FirstName == playerName).Select(pl => new
            {
                FullName = pl.FirstName + " " + pl.LastName,
                Age = pl.Age,
                Salary = pl.Salary,
                CountryName = pl.Country.Name,
                TeamName = pl.Team.Name,
                ManagerName = pl.Team.Manager,
                StadiumName = pl.Team.Stadium,
                CityName = pl.Team.City.Name


            });

            foreach (var p in player)
            {
                Console.Write($@"
   Name:    { p.FullName}
   Age:     {p.Age}
   Country: {p.CountryName}
   Salary:  {p.Salary}
   Team:    {p.TeamName}
   Manager: {p.ManagerName}
   Stadium: {p.StadiumName}
   City:    {p.CityName} 
   ");
            }

            Console.WriteLine(new String('*', 50));
        }

        static void EditPlayerSalary()
        {
            var db = new FootballDbContext();

            Console.Write("Player name to change salary: ");
            var playerName = Console.ReadLine();

            var player = db.Players.SingleOrDefault(p => p.FirstName == playerName);

            if (player == null)
            {
                Console.WriteLine($"No player with that name!");
                Console.WriteLine(new String('*', 50));
            }
            else
            {
                Console.Write("Enter new salary: ");
                var newSalary = decimal.Parse(Console.ReadLine());
                player.Salary = newSalary;

                db.SaveChanges();
                Console.WriteLine($"Player {player.FirstName} {player.LastName} has a new salary!");
                Console.WriteLine(new String('*', 50));
            }

        }

        static void exportToXML()
        {
            var context = new FootballDbContext();
            var playerQuery = context.Players.Select(pl => new
            {
                FullName = pl.FirstName + " " + pl.LastName,
                Age = pl.Age,
                Salary = pl.Salary,
                CountryName = pl.Country.Name,
                TeamName = pl.Team.Name,
                ManagerName = pl.Team.Manager,
                StadiumName = pl.Team.Stadium,
                CityName = pl.Team.City.Name
            });

            var xmlPlayers = new XElement("Players");

            foreach (var player in playerQuery)
            {
                var curPlayer = new XElement("Player",
                                    new XElement("age", player.Age),
                                    new XElement("salary", player.Salary),
                                    new XElement("country", player.CountryName),
                                    new XElement("team",
                                        new XAttribute("name", player.TeamName),
                                        new XElement("manager", player.ManagerName),
                                        new XElement("stadium", player.StadiumName),
                                        new XElement("city", player.CityName)
                                        )
                                    );
                curPlayer.Add(new XAttribute("name", player.FullName));
                xmlPlayers.Add(curPlayer);
                xmlPlayers.Add(new XElement("age", player.Age));
            }

            var xmlDoc = new XDocument(xmlPlayers);
            xmlDoc.Save("Players.xml");
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
            Console.WriteLine("5. Find player by name");
            Console.WriteLine("6. Edit player salary by name");
            Console.WriteLine("7. Export players to xml");
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
                else if (commandNumber == 5)
                {
                    FindPlayer();
                }
                else if (commandNumber == 6)
                {
                    EditPlayerSalary();
                }
                else if (commandNumber == 7)
                {
                    exportToXML();
                }
                else
                {
                    Console.WriteLine("Wrong command number!!!");
                }


            }
        }
    }
}
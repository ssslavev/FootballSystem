namespace FootballSystem.ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using Data;
    using Data.Migrations;

    using Excel;

    using Models;

    using Newtonsoft.Json;

    public class Startup
    {
        private static readonly FootballDbContext DbContext = new FootballDbContext();

        private static void ImportCountriesFromJsontoSqlServer()
        {
            var fileBrowser = new OpenFileDialog
            {
                Filter = "JSON Document|*.json"
            };

            if (fileBrowser.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var file = File.ReadAllText(fileBrowser.FileName);
            
            var countriesToAdd = JsonConvert.DeserializeObject<IEnumerable<ParseModel>>(file).ToList();
            
            var counter = 0;

            foreach (var country in countriesToAdd)
            {
                var countryToAdd = new Country
                {
                    Name = country.Name
                };

                DbContext.Countries.Add(countryToAdd);
                counter++;
            }

            DbContext.SaveChanges();

            Console.WriteLine($"{counter} countries was added!");
            Console.WriteLine(new string('*', 50));
        }

        private static void ImportCitiesFromExcellToSqlServer()
        {
            FileStream stream = File.Open(Directory.GetCurrentDirectory() + "/cities.xlsx", FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();

            DataTable dt = result.Tables[0];

            var counter = 0;

            foreach (DataRow row in dt.Rows)
            {
                string countryName = row.Field<string>(1).Trim();

                var country = DbContext.Countries.FirstOrDefault(co => co.Name == countryName);

                var cityToAdd = new City
                {
                    Name = row.Field<string>(0).Trim(),
                    Country = country
                };

                DbContext.Cities.Add(cityToAdd);
                counter++;
            }

            DbContext.SaveChanges();
            excelReader.Close();

            Console.WriteLine($"{counter} cities was added!");
            Console.WriteLine(new string('*', 50));
        }

        private static void AddPlayer()
        {
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

            DbContext.Players.Add(player);
            DbContext.SaveChanges();
            Console.WriteLine($"Player {firstName} {lastName} was added!");
            Console.WriteLine(new string('*', 50));
        }

        private static void RemovePlayer()
        {
            Console.Write("Player name: ");
            var playerName = Console.ReadLine();

            var row = DbContext.Database.ExecuteSqlCommand($"DELETE FROM Players WHERE FirstName = '{playerName}'");

            if (row == 1)
            {
                Console.WriteLine($"The player {playerName} was deleted!");
                Console.WriteLine(new string('*', 50));
            }
            else
            {
                Console.WriteLine($"No player with that name!");
                Console.WriteLine(new string('*', 50));
            }
        }

        private static void FindPlayer()
        {
            Console.Write("Player name: ");
            var playerName = Console.ReadLine();

            var player = DbContext.Players.Where(pl => pl.FirstName == playerName).Select(pl => new
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
   Name:    {p.FullName}
   Age:     {p.Age}
   Country: {p.CountryName}
   Salary:  {p.Salary}
   Team:    {p.TeamName}
   Manager: {p.ManagerName}
   Stadium: {p.StadiumName}
   City:    {p.CityName} 
   ");
            }

            Console.WriteLine(new string('*', 50));
        }

        private static void EditPlayerSalary()
        {
            Console.Write("Player name to change salary: ");
            var playerName = Console.ReadLine();

            var player = DbContext.Players.SingleOrDefault(p => p.FirstName == playerName);

            if (player == null)
            {
                Console.WriteLine($"No player with that name!");
                Console.WriteLine(new string('*', 50));
            }
            else
            {
                Console.Write("Enter new salary: ");
                var newSalary = decimal.Parse(Console.ReadLine());
                player.Salary = newSalary;

                DbContext.SaveChanges();
                Console.WriteLine($"Player {player.FirstName} {player.LastName} has a new salary!");
                Console.WriteLine(new string('*', 50));
            }
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1116:SplitParametersMustStartOnLineAfterDeclaration", Justification = "Reviewed. Suppression is OK here.")]
        private static void ExportToXml()
        {
            var playerQuery = DbContext.Players.Select(
                pl => new
                {
                    FullName = pl.FirstName + " " + pl.LastName,
                    Age = pl.Age,
                    Salary = pl.Salary,
                    CountryName = pl.Country.Name,
                    TeamName = pl.Team.Name,
                    TeamCountryName = pl.Team.Country.Name,
                    ManagerName = pl.Team.Manager,
                    StadiumName = pl.Team.Stadium,
                    Championshp = pl.Team.Championship.Name,
                    CityName = pl.Team.City.Name,
                    TeamCountry = pl.Team.Country.Name
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
                                        new XElement("city", player.CityName),
                                        new XElement("country", player.TeamCountryName),
                                        new XElement("championship", player.Championshp),
                                        new XElement("country", player.TeamCountry)));
                curPlayer.Add(new XAttribute("name", player.FullName));
                xmlPlayers.Add(curPlayer);
            }

            var xmlDoc = new XDocument(xmlPlayers);
            xmlDoc.Save("Players.xml");
        }

        private static void ImportFromXml()
        {
            var xmlDoc = XDocument.Load(@"players.xml");
            var playerNodes = xmlDoc.XPathSelectElements("Players/Player");
            var db = new FootballDbContext();

            foreach (var pl in playerNodes)
            {
                string[] fullName = pl.Attribute("name").Value.Split();
                int age = int.Parse(pl.Element("age").Value);
                decimal salary = decimal.Parse(pl.Element("salary").Value);
                string country = pl.Element("country").Value;
                string teamName = pl.Element("team").Attribute("name").Value;
                string manager = pl.Element("team").Element("manager").Value;
                string stadium = pl.Element("team").Element("stadium").Value;
                string city = pl.Element("team").Element("city").Value;
                string championship = pl.Element("team").Element("championship").Value;
                string teamCountry = pl.Element("team").Element("country").Value;

                var player = new Player
                {
                    FirstName = fullName[0],
                    LastName = fullName[1],
                    Age = age,
                    Salary = salary,
                    Country = new Country { Name = country },
                    Team = new Team
                    {
                        Name = teamName,
                        Manager = manager,
                        Stadium = stadium,
                        Country = new Country { Name = teamCountry },
                        Championship = new Championship { Name = championship },
                        City = new City { Name = city }
                    }
                };

                db.Players.Add(player);
                db.SaveChanges();
            }
        }

        [STAThread]
        private static void Main()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FootballDbContext, Configuration>());
            
            Console.WriteLine("1. Import countries from json file to sql server");
            Console.WriteLine("2. Import cities from excell to sql server");
            Console.WriteLine("3. Add new player");
            Console.WriteLine("4. Remove player by player first name");
            Console.WriteLine("5. Find player by name");
            Console.WriteLine("6. Edit player salary by name");
            Console.WriteLine("7. Export players to xml");
            Console.WriteLine("8. Import data from xml");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Enter the number of the command you want!");
                Console.Write("number:  ");

                var commandNumber = int.Parse(Console.ReadLine());

                switch (commandNumber)
                {
                    case 1:
                        ImportCountriesFromJsontoSqlServer();
                        break;
                    case 2:
                        ImportCitiesFromExcellToSqlServer();
                        break;
                    case 3:
                        AddPlayer();
                        break;
                    case 4:
                        RemovePlayer();
                        break;
                    case 5:
                        FindPlayer();
                        break;
                    case 6:
                        EditPlayerSalary();
                        break;
                    case 7:
                        ExportToXml();
                        break;
                    case 8:
                        ImportFromXml();
                        break;
                    default:
                        Console.WriteLine("Wrong command number!!!");
                        break;
                }
            }
        }
    }
}
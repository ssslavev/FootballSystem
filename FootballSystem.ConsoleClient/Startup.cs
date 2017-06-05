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

        static void Main()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FootballDbContext, Configuration>());

            var postgresqlDb = new FootballPostgresqlDbContext();



            Console.WriteLine("1. Import countries and cities from json and excel.");
            Console.WriteLine("2. Import players from sql server to postgresql");
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
            }
        }
    }
}









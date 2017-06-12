using Excel;
using FootballSystem.ConsoleClient;
using FootballSystem.Data;
using FootballSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FootballSystem.WindowsFormsClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        void FillCountries()
        {
            var db = new FootballDbContext();

            var countries = db.Countries.ToList();


            foreach (var country in countries)
            {
                comboBox1.Items.Add(country.Name);
                comboBox2.Items.Add(country.Name);
            }

            db.Dispose();
        }

        void FillCities()
        {
            var db = new FootballDbContext();

            var cities = db.Cities.ToList();

            foreach (var city in cities)
            {
                comboBox3.Items.Add(city.Name);
            }

            db.Dispose();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            var db = new FootballDbContext();

            var player = new Player
            {
                FirstName = textBox1.Text,
                LastName = textBox2.Text,
                Age = int.Parse(textBox3.Text),
                Salary = int.Parse(textBox4.Text),
                Country = new Country { Name = comboBox1.Text },
                Team = new Team
                {
                    Name = textBox5.Text,
                    Manager = textBox7.Text,
                    Stadium = textBox6.Text,
                    Country = new Country { Name = comboBox2.Text },
                    Championship = new Championship { Name = textBox8.Text },
                    City = new City { Name = comboBox3.Text }
                }

            };

            db.Players.Add(player);
            db.SaveChanges();
            MessageBox.Show($"Player {player.FirstName} {player.LastName} was created!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
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

            MessageBox.Show($"{counter} countries was added!");
            FillCountries();
        }

        private void button3_Click(object sender, EventArgs e)
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

            MessageBox.Show($"{counter} cities was added!");

            FillCities();
        }
    }
}

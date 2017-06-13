namespace FootballSystem.WindowsFormsClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    using Excel;

    using FootballSystem.ConsoleClient;
    using FootballSystem.Data;
    using FootballSystem.Models;

    using Newtonsoft.Json;

    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();

            Form3 form3 = new Form3();
            form2.Show();

            Form4 form4 = new Form4();
            form2.Show();
        }

        private void FillCountries()
        {
            var db = new FootballDbContext();

            var countries = db.Countries.ToList();

            foreach (var country in countries)
            {
                this.comboBox1.Items.Add(country.Name);
                this.comboBox2.Items.Add(country.Name);
            }

            db.Dispose();
        }

        private void FillCities()
        {
            var db = new FootballDbContext();

            var cities = db.Cities.ToList();

            foreach (var city in cities)
            {
                this.comboBox3.Items.Add(city.Name);
            }

            db.Dispose();
        }

        private void AddPlayerClick(object sender, EventArgs e)
        {
            var dbContext = new FootballDbContext();

            var country = dbContext.Countries.FirstOrDefault(c => c.Name == this.comboBox1.Text)
                          ?? new Country { Name = this.comboBox1.Text };
            var teamCountry = dbContext.Countries.FirstOrDefault(c => c.Name == this.comboBox2.Text)
                          ?? new Country { Name = this.comboBox2.Text };

            var player = new Player
            {
                FirstName = this.textBox1.Text,
                LastName = this.textBox2.Text,
                Age = int.Parse(this.textBox3.Text),
                Salary = int.Parse(this.textBox4.Text),
                Country = country,
                Team = new Team
                {
                    Name = this.textBox5.Text,
                    Manager = this.textBox7.Text,
                    Stadium = this.textBox6.Text,
                    Country = teamCountry,
                    Championship = new Championship { Name = this.textBox8.Text },
                    City = new City { Name = this.comboBox3.Text }
                }
            };

            dbContext.Players.Add(player);
            dbContext.SaveChanges();
            MessageBox.Show($@"Player {player.FirstName} {player.LastName} was created!");
        }

        private void ImportCountriesClick(object sender, EventArgs e)
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

            MessageBox.Show($@"{counter} countries was added!");
            this.FillCountries();
        }

        private void ImportCitiesClick(object sender, EventArgs e)
        {
            var db = new FootballDbContext();

            // import cities from excell file
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

            MessageBox.Show($@"{counter} cities was added!");

            this.FillCities();
        }

        private void SearchPlayerClick(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void DeletePlayerClick(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }
    }
}

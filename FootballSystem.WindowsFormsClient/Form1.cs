using FootballSystem.Data;
using FootballSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            FillCombo();
        }

        void FillCombo()
        {
            var db = new FootballDbContext();

            var countries = db.Countries.ToList();
            var cities = db.Cities.ToList();

            foreach (var country in countries)
            {
                comboBox1.Items.Add(country.Name);
                comboBox2.Items.Add(country.Name);
            }

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
    }
}

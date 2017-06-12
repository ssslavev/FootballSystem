using FootballSystem.Data;
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var db = new FootballDbContext();


            var playerName = textBox1.Text;

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

            var str = "";

            foreach (var p in player)
            {
                 str = $@"
   Name:    { p.FullName}
   Age:     {p.Age}
   Country: {p.CountryName}
   Salary:  {p.Salary}
   Team:    {p.TeamName}
   Manager: {p.ManagerName}
   Stadium: {p.StadiumName}
   City:    {p.CityName} 
   ";
            }


            label2.Text = str;
        }
    }
}

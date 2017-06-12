namespace FootballSystem.WindowsFormsClient
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    using FootballSystem.Data;

    public partial class Form2 : Form
    {
        public Form2()
        {
            this.InitializeComponent();
        }

        private void FindClick(object sender, EventArgs e)
        {
            var db = new FootballDbContext();

            var playerName = this.textBox1.Text;

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

            var str = string.Empty;

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

            this.label2.Text = str;
        }
    }
}

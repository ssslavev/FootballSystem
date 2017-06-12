namespace FootballSystem.WindowsFormsClient
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    using FootballSystem.Data;

    public partial class Form4 : Form
    {
        public Form4()
        {
            this.InitializeComponent();
        }

        private void UpdateClick(object sender, EventArgs e)
        {
            var db = new FootballDbContext();

            var playerName = this.textBox1.Text;

            var player = db.Players.SingleOrDefault(p => p.FirstName == playerName);

            if (player == null)
            {
                MessageBox.Show(@"No player with that name!");   
            }
            else
            {
                var newSalary = decimal.Parse(this.textBox2.Text);
                player.Salary = newSalary;

                db.SaveChanges();
                MessageBox.Show($@"Player {player.FirstName} {player.LastName} has a new salary!");
            }
        }
    }
}

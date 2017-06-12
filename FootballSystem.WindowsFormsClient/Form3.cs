namespace FootballSystem.WindowsFormsClient
{
    using System;
    using System.Windows.Forms;

    using Data;

    public partial class Form3 : Form
    {
        public Form3()
        {
            this.InitializeComponent();
        }

        private void DeleteClick(object sender, EventArgs e)
        {
            var db = new FootballDbContext();

            var playerName = this.textBox1.Text;

            var row = db.Database.ExecuteSqlCommand($"DELETE FROM Players WHERE FirstName = '{playerName}'");

            if (row > 0)
            {
                MessageBox.Show($@"Player {playerName} was deleted!");
            }
            else
            {
                MessageBox.Show(@"No player with that name!");
            }
        }
    }
}

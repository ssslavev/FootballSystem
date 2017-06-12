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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var db = new FootballDbContext();

            var playerName = textBox1.Text;

            var player = db.Players.SingleOrDefault(p => p.FirstName == playerName);

            if (player == null)
            {
                MessageBox.Show($"No player with that name!");
                
            }
            else
            {

                var newSalary = decimal.Parse(textBox2.Text);
                player.Salary = newSalary;

                db.SaveChanges();
                MessageBox.Show($"Player {player.FirstName} {player.LastName} has a new salary!");
                
            }
        }
    }
}

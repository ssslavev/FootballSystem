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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var db = new FootballDbContext();


            var playerName = textBox1.Text;

            var row = db.Database.ExecuteSqlCommand($"DELETE FROM Players WHERE FirstName = '{playerName}'");

            if (row> 0)
            {
                MessageBox.Show($"Player {playerName} was deleted!");
            }
            else
            {
                MessageBox.Show($"No player with that name!");
               
            }
        }
    }
}

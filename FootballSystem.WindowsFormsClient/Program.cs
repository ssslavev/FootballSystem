namespace FootballSystem.WindowsFormsClient
{
    using System;
    using System.Data.Entity;
    using System.Windows.Forms;

    using Data;
    using Data.Migrations;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FootballDbContext, Configuration>());

            //// var db = new FootballDbContext();
            //// db.Players.Count();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Application.Run(new Form2());
            Application.Run(new Form3());
            Application.Run(new Form4());
        }
    }
}

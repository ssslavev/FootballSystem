namespace FootballSystem.Data
{
    using System.Data.Entity;

    using Models;

    internal class ModelCreating
    {
        internal static void AllModels(DbModelBuilder modelBuilder)
        {
            OnCityModelCreating(modelBuilder);
            OnCoachModelCreating(modelBuilder);
        }

        private static void OnCityModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasRequired(c => c.Country)
                .WithMany(c => c.Cities)
                .HasForeignKey(c => c.CountryId)
                .WillCascadeOnDelete(false);
        }

        private static void OnCoachModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Coaches)
                .WithMany(c => c.Players)
                .Map(cp =>
                    {
                        cp.MapLeftKey("PlayerId");
                        cp.MapRightKey("CoachId");
                        cp.ToTable("PlayerCoach");
                    });
        }
    }
}

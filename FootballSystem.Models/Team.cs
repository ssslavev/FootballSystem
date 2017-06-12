namespace FootballSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        private ICollection<Player> players;

        public Team()
        {
            this.players = new HashSet<Player>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Manager { get; set; }

        [MinLength(2)]
        [MaxLength(20)]
        public string Stadium { get; set; }

        public virtual ICollection<Player> Players
        {
            get { return this.players; }
            set { this.players = value; }
        }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; }

        public int ChampionshipId { get; set; }

        public virtual Championship Championship { get; set; }
    }
}

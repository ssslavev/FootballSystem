namespace FootballSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Advertisements
    {
        private ICollection<Player> players;

        public Advertisements()
        {
            this.players = new HashSet<Player>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }

        public ICollection<Player> Players
        {
            get
            {
                return this.players;
            }

            private set
            {
                players = value;
            }
        }
    }
}
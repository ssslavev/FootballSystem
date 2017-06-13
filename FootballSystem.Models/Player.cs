namespace FootballSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Player
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public int Age { get; set; }

        public int? TeamId { get; set; }

        public virtual Team Team { get; set; }

        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }

        public ICollection<Advertisements> Advertisements { get; set; }
    }
}

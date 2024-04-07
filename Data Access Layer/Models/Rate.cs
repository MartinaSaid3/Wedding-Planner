using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class Rate
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // Rating on a scale of 1 to 5

        [Required]
        public int UserId { get; set; }

        [ForeignKey("Venue")]
        [Required]
        public int VenueId { get; set; } // Foreign key


        public Venue Venue { get; set; } // Reference navigation property
    }
}

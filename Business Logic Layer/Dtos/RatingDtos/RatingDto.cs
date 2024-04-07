using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Dtos.RatingDtos
{
    public class RatingDto
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public int VenueId { get; set; }

        [Required]
        public int UserId { get; set; }

    }
}

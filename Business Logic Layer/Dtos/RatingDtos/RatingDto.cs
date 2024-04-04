using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Dtos.RatingDtos
{
    public class RatingDto
    {
        public int ReservationId { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
    }
}

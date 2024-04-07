using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Dtos.ReservationDtos
{
    public class ReservationWithTotalPriceDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int NumOfGuests { get; set; }
        //forgien key venue
        [Required]
        public int VenueId { get; set; }

        public string SpecialRequests { get; set; }
        [Required]
        public string Email { get; set; }
        public string Service { get; set; }
        public double TotalPrice { get; set; }
    }
}

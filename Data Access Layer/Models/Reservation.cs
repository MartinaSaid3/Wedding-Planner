using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int NumOfGuests { get; set; }

        //forgien key venue
        [ForeignKey("Venue")]
        public int VenueId { get; set; }

        public string SpecialRequests { get; set; }
        public string Email { get; set; }

        //navigation property
        public Venue? Venue { get; set; }

    }
}

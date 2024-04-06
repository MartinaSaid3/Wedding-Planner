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
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public int NumOfGuests { get; set; }

        //forgien key venue
        [ForeignKey("Venue")]
        public int VenueId { get; set; }

        public string SpecialRequests { get; set; }
        public string Email { get; set; }
        public string Service { get; set; }
        public double TotalPrice { get; set; }


        public ApprovalStatusReservation Status { get; set; }
        public Reservation()
        {
            // Initialize status to Pending by default
            Status = ApprovalStatusReservation.Pending;
        }

        public enum ApprovalStatusReservation
        {
            Pending,
            Accepted,
            Rejected
        }
        

        //navigation property
        public Venue? Venue { get; set; }

        //public string UniqueToken { get; set; } // Property for the unique token

    }
}

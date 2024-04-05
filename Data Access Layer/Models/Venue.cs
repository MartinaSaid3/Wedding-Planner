using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class Venue
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }


        [Required]
        [RegularExpression(@"^[\w\s]+,[\w\s]+,[\w\s]+$", ErrorMessage = "Location format should be 'city, area, street'.")]
        public string Location { get; set; }


        [Required]
        public string Description { get; set; }


        [Required]
        public string OpenBuffet { get; set; }

        [Required]
        public string SetMenue { get; set; }

        [Required]
        public string HighTea { get; set; }

        [Required]
        public double MaxCapacity { get; set; }
        [Required]
        public double MinCapacity { get; set; }


        [Required]
        public double PriceOpenBuffetPerPerson { get; set; }

        [Required]
        public double PriceSetMenuePerPerson { get; set; }

        [Required]
        public double PriceHighTeaPerPerson { get; set; }

        public double MinPrice
        {
            get
            {
                return PriceHighTeaPerPerson * MinCapacity;
            }
            set
            {

            }
        }

        //public double TotalPrice { get; set; }


        [Required]
        public List<string> ImagesData { get; set; }


        //3shan maidfsh elreservation nafsha marten,, bt5ly el obj mawgoud mara w7da fel list
        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();

        public ApprovalStatus Status { get; set; }

        // Constructor
        public Venue()
        {
            // Initialize status to Pending by default
            Status = ApprovalStatus.Pending;
        }
        public enum ApprovalStatus
        {
            Pending,
            Accepted,
            Rejected
        }
    }
}

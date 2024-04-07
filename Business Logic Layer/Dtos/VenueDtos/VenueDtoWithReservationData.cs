using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Dtos.VenueDtos
{
    public class VenueDtoWithReservationData
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }


        [Required]
        [RegularExpression(@"^[\w\s]+,[\w\s]+,[\w\s]+$", ErrorMessage = "Location format should be 'city, area, street'.")]
        public string Location { get; set; }


        [Required]
        public string Description { get; set; }


        //[Required]
        //public double PriceOpenBuffetPerPerson { get; set; }

        //[Required]
        //public double PriceSetMenuePerPerson { get; set; }

        //[Required]
        //public double PriceHighTeaPerPerson { get; set; }


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

        public double PriceStartingFrom { get; set; }


        //[Required]
        //public List<byte[]> Images { get; set; }

        public List<DateTime> ReservationDates { get; set; }
        public List<string> ImagesData { get; set; }

        public double AverageRate { get; set; }
    }
}

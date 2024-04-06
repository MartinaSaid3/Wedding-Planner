using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Dtos.VenueDtos
{
    public class VenueWithReservationUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string OpenBuffet { get; set; }


        public string SetMenue { get; set; }


        public string HighTea { get; set; }

        public double MaxCapacity { get; set; }

        public double MinCapacity { get; set; }


        public double PriceStartingFrom { get; set; }


        //reference type hatb2a null lw 3mlt add feha hatdrab exception 
        public List<string> ReservationUser { get; set; } = new List<string>();

        public List<string> ImagesData { get; set; }
    }
}

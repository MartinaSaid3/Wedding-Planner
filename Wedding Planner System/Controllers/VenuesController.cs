using Business_Logic_Layer.Dtos.VenueDtos;
using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Wedding_Planner_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly ApplicationEntity Context;

        public VenuesController(ApplicationEntity _context)
        {
            Context = _context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllVenue()
        {
            List<Venue> VenueList = await Context.Venues.
                Include(a => a.Reservations).ToListAsync();
            if (VenueList.Count > 0)
            {
                List<VenueWithReservationIdDto> ListOfVenueDto = new List<VenueWithReservationIdDto>();

                foreach (var item in VenueList)
                {

                    VenueWithReservationIdDto venueDto = new VenueWithReservationIdDto
                    {
                        Name = item.Name,
                        Description = item.Description,
                        Location = item.Location,
                        OpenBuffet = item.OpenBuffet,
                        SetMenue = item.SetMenue,
                        HighTea = item.HighTea,
                        MaxCapacity = item.MaxCapacity,
                        MinCapacity = item.MinCapacity,
                        PriceStartingFrom = item.MinPrice,

                    };

                    ListOfVenueDto.Add(venueDto);
                }
                return Ok(ListOfVenueDto);

            }
            return BadRequest("there are no halls");

        }


        // hai3red el reesrvations id m3ah
        [HttpGet]
        [Route("{id:int}", Name = "GetOneVenueRoute")]
        public IActionResult GetVenueById(int id)
        {
            if (id == null)
            {
                return BadRequest("must enter id ");
            }
            Venue v1 = Context.Venues.Include(a => a.Reservations).FirstOrDefault(a => a.Id == id);

            if (v1 != null)
            {
                VenueWithReservationIdDto VenueDto = new VenueWithReservationIdDto();
                VenueDto.Name = v1.Name;
                VenueDto.Description = v1.Description;
                VenueDto.Location = v1.Location;
                VenueDto.OpenBuffet = v1.OpenBuffet;
                VenueDto.SetMenue = v1.SetMenue;
                VenueDto.HighTea = v1.HighTea;
                VenueDto.PriceStartingFrom = v1.MinPrice;
                VenueDto.MinCapacity = v1.MinCapacity;
                VenueDto.MaxCapacity = v1.MaxCapacity;

                foreach (var item in v1.Reservations)
                {
                    VenueDto.ReservationId.Add(item.Id);
                }

                return Ok(VenueDto);
            }
            return BadRequest("This Venue DoesNot Exist");

        }



        [HttpPost]
        [Route("/{venueId:int}/{selectedService:alpha}/{numberOfGuests:int}")]
        public async Task<IActionResult> CalculateTotalPrice(int venueId, string selectedService, int numberOfGuests)
        {

            Venue venue = await Context.Venues.FirstOrDefaultAsync(v => v.Id == venueId);

            if (venue == null)
            {
                return NotFound("Venue not found.");
            }

            double totalPrice = 0;

            // Trim and convert selectedService to lower case for case-insensitive comparison
            selectedService = selectedService.Trim().ToLower();

            // Validate the selected service
            switch (selectedService)
            {
                case "openbuffet":
                    totalPrice = numberOfGuests * venue.PriceOpenBuffetPerPerson;
                    break;
                case "setmenue":
                    totalPrice = numberOfGuests * venue.PriceSetMenuePerPerson;
                    break;
                case "hightea":
                    totalPrice = numberOfGuests * venue.PriceHighTeaPerPerson;
                    break;
                default:
                    return BadRequest("Invalid service selected.");
            }

            //venue.TotalPrice = totalPrice;
            //await Context.Venues.AddAsync(venue);
            //await Context.SaveChangesAsync();

            return Ok($"Total price for {numberOfGuests} guests: {totalPrice}");
        }


        // hai3red el reservations dates
        [HttpGet]
        [Route("Name/{Name:alpha}", Name = "GetByName")]
        public async Task<IActionResult> GetVenueByName(string Name)
        {
            if (Name == null)
            {
                return BadRequest("you must enter name");
            }

            List<Venue> VenueList = await Context.Venues
                .Include(v => v.Reservations)
                .Where(a => a.Name == Name).ToListAsync();

            if (VenueList.Count == 0)
            {
                BadRequest("There are no Venue with the same name");
            }

            List<VenueDtoWithReservationData> ListVenueDTO = new List<VenueDtoWithReservationData>();

            foreach (var item in VenueList)
            {
                VenueDtoWithReservationData venueDto = new VenueDtoWithReservationData
                {
                    Name = item.Name,
                    Description = item.Description,
                    Location = item.Location,
                    OpenBuffet = item.OpenBuffet,
                    SetMenue = item.SetMenue,
                    HighTea = item.HighTea,
                    MaxCapacity = item.MaxCapacity,
                    MinCapacity = item.MinCapacity,
                    PriceStartingFrom = item.MinPrice,
                    ReservationDates = new List<DateTime>() // Initialize the list
                };

                foreach (var reservation in item.Reservations)
                {
                    venueDto.ReservationDates.Add(reservation.Date);
                }


                ListVenueDTO.Add(venueDto);
            }

            return Ok(ListVenueDTO);

        }


        [HttpGet]
        [Route("price/{price:int}", Name = "GetByPrice")]
        public async Task<IActionResult> GetVenueByPrice(double price)
        {
            if (price == null)
            {
                return BadRequest("you must enter price");
            }

            List<Venue> VenueList = await Context.Venues.
                Include(v => v.Reservations)
                .Where(a => a.MinPrice <= price).ToListAsync();

            if (VenueList.Count == 0)
            {
                BadRequest("There are no Venue with the same price");
            }

            List<VenueDtoWithReservationData> ListVenueDTO = new List<VenueDtoWithReservationData>();

            foreach (var item in VenueList)
            {
                VenueDtoWithReservationData VenueDto = new VenueDtoWithReservationData
                {
                    Name = item.Name,
                    Description = item.Description,
                    Location = item.Location,
                    OpenBuffet = item.OpenBuffet,
                    SetMenue = item.SetMenue,
                    HighTea = item.HighTea,
                    MaxCapacity = item.MaxCapacity,
                    MinCapacity = item.MinCapacity,
                    PriceStartingFrom = item.MinPrice,
                    ReservationDates = new List<DateTime>() // Initialize the list
                };

                foreach (var reservation in item.Reservations)
                {
                    VenueDto.ReservationDates.Add(reservation.Date);
                }
                ListVenueDTO.Add(VenueDto);
            }

            return Ok(ListVenueDTO);

        }



        //[HttpGet]
        //[Route("/PriceAndLocation{price:int}/{location:alpha}")]
        //public async Task< IActionResult> GetVenueByLocationAndPrice(double price, string location)
        //{
        //    if (ModelState.IsValid)
        //    {
        //      List<Venue> ListVenue = await Context.Venues.Include(v=>v.Reservations).
        //            Where(Venue =>
        //      Venue.MinPrice <= price &&
        //      Venue.Location.Contains(location)).ToListAsync();



        //        if (ListVenue.Count == 0)
        //        {
        //            return BadRequest("no venues with the same price and location");
        //        }


        //        List<VenueDtoWithReservationData> ListVenueDto = new List<VenueDtoWithReservationData>();

        //        foreach (var venue in ListVenue)
        //        {
        //            VenueDtoWithReservationData venueDto = new VenueDtoWithReservationData
        //            {
        //                Name = venue.Name,
        //                Description = venue.Description,
        //                Location = venue.Location,
        //                MaxCapacity = venue.MaxCapacity,
        //                MinCapacity = venue.MinCapacity,
        //                MaxPrice = venue.MaxPrice,
        //                MinPrice = venue.MinPrice,
        //                ReservationDates = new List<DateTime>()


        //            };

        //            foreach(var reservation in venue.Reservations)
        //            {
        //                venueDto.ReservationDates.Add(reservation.ReservationDate);
        //            }

        //            ListVenueDto.Add(venueDto);
        //        }
        //        return Ok(ListVenueDto);
        //    }
        //    return BadRequest(ModelState);
        //}






        [HttpGet]
        [Route("{Location:alpha}")]
        public async Task<IActionResult> GetVenueByLocation(string Location)
        {


            if (string.IsNullOrWhiteSpace(Location))
            {
                return BadRequest("You must enter a location");
            }

            List<Venue> VenueList = await Context.Venues.
                Include(v => v.Reservations)
               .Where(a => a.Location.Contains(Location)).ToListAsync();

            if (VenueList == null || VenueList.Count == 0)
            {
                BadRequest("There are no Venue with the same city");
            }

            List<VenueDtoWithReservationData> ListVenueDTO = new List<VenueDtoWithReservationData>();

            foreach (var item in VenueList)
            {
                VenueDtoWithReservationData VenueDto = new VenueDtoWithReservationData
                {
                    Name = item.Name,
                    Description = item.Description,
                    Location = item.Location,
                    OpenBuffet = item.OpenBuffet,
                    SetMenue = item.SetMenue,
                    HighTea = item.HighTea,
                    MaxCapacity = item.MaxCapacity,
                    MinCapacity = item.MinCapacity,
                    PriceStartingFrom = item.MinPrice,

                    ReservationDates = new List<DateTime>() // Initialize the list
                };


                foreach (var reservation in item.Reservations)
                {
                    VenueDto.ReservationDates.Add(reservation.Date);
                }
                ListVenueDTO.Add(VenueDto);
            }

            return Ok(ListVenueDTO);

        }



        [HttpPost]
        public async Task<IActionResult> SaveVenue(VenueDto VenueDto)
        {
            if (ModelState.IsValid)
            {
                if (VenueDto.ImagesData == null || VenueDto.ImagesData.Count == 0)
                {
                    return BadRequest("No images provided.");
                }
                Venue venue = new Venue();
                venue.Name = VenueDto.Name;
                venue.Description = VenueDto.Description;
                venue.Location = VenueDto.Location;
                venue.OpenBuffet = VenueDto.OpenBuffet;
                venue.SetMenue = VenueDto.SetMenue;
                venue.HighTea = VenueDto.HighTea;
                venue.PriceOpenBuffetPerPerson = VenueDto.PriceOpenBuffetPerPerson;
                venue.PriceSetMenuePerPerson = VenueDto.PriceSetMenuePerPerson;
                venue.PriceHighTeaPerPerson = VenueDto.PriceHighTeaPerPerson;
                venue.MinCapacity = VenueDto.MinCapacity;
                venue.MaxCapacity = VenueDto.MaxCapacity;
                venue.ImagesData = await ConvertImagesToByteArray(VenueDto.ImagesData); // Convert images to byte array




                // Save the venue to the database
                Context.Venues.Add(venue);
                await Context.SaveChangesAsync();


                // Get the URL for the newly created venue
                string url = Url.Link("GetOneVenueRoute", new { id = venue.Id });
                return Created(url, venue);

            }

            return BadRequest(ModelState);


        }

        private async Task<List<byte[]>> ConvertImagesToByteArray(List<IFormFile> imageFiles)
        {
            var imageDataList = new List<byte[]>();

            foreach (var imageFile in imageFiles)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        imageDataList.Add(memoryStream.ToArray());
                    }
                }
            }

            return imageDataList;
        }



        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateVenue([FromRoute] int id, [FromBody] VenueDto VenueDto)
        {
            if (ModelState.IsValid)
            {
                Venue OldVenue = await Context.Venues.FirstOrDefaultAsync(a => a.Id == id);

                if (OldVenue != null)
                {
                    OldVenue.Name = VenueDto.Name;
                    OldVenue.Description = VenueDto.Description;
                    OldVenue.Location = VenueDto.Location;
                    OldVenue.OpenBuffet = VenueDto.OpenBuffet;
                    OldVenue.SetMenue = VenueDto.SetMenue;
                    OldVenue.HighTea = VenueDto.HighTea;
                    OldVenue.PriceOpenBuffetPerPerson = VenueDto.PriceOpenBuffetPerPerson;
                    OldVenue.PriceSetMenuePerPerson = VenueDto.PriceSetMenuePerPerson;
                    OldVenue.PriceHighTeaPerPerson = VenueDto.PriceHighTeaPerPerson;
                    OldVenue.MaxCapacity = VenueDto.MaxCapacity;
                    OldVenue.MinCapacity = VenueDto.MinCapacity;


                    await Context.SaveChangesAsync();
                    return StatusCode(204, OldVenue);
                }
                return BadRequest("Id Not Valid");


            }
            return BadRequest(ModelState);
        }



        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> RemoveVenue(int id)
        {
            if (id == null)
            {
                return BadRequest("please enter id");
            }

            Venue venue = await Context.Venues.FirstOrDefaultAsync(a => a.Id == id);

            if (venue != null)
            {
                try
                {
                    Context.Venues.Remove(venue);
                    Context.SaveChanges();
                    return StatusCode(204, "venue removed successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            return BadRequest("there is no venue with same id");

        }

    }
}

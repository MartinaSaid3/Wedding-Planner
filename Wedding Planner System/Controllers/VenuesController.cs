using Business_Logic_Layer.Dtos.VenueDtos;
using Business_Logic_Layer.Service.VenueService;
using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repo.VenueRepo;
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
       
        private readonly IVenueBLL venueBLL;

        public VenuesController(IVenueBLL _VenueBLL)
        {
            
            venueBLL = _VenueBLL;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueWithReservationIdDto>>> GetAllVenues()
        {
            try
            {
                List<VenueWithReservationIdDto> venues = await venueBLL.GetAllVenue();
                return Ok(venues);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // hai3red el reesrvations id m3ah
        //[Authorize(Roles = "client,ServiceProvider")]
        [HttpGet]
        [Route("{id:int}", Name = "GetOneVenueRoute")]
        public async Task<ActionResult<VenueDtoWithReservationData>> GetVenueById(int id)
        {
            try
            {
                VenueDtoWithReservationData venueDto = await venueBLL.GetVenueById(id);
                return Ok(venueDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }



        //[Authorize(Roles = "client")]
        [Route("/{venueId:int}/{selectedService:alpha}/{numberOfGuests:int}")]
        [HttpGet]
        public async Task<IActionResult> CalculateTotalPrice(int venueId, string selectedService, int numberOfGuests)
        {
            try
            {
                double totalPrice = await venueBLL.CalculateTotalPrice(venueId, selectedService, numberOfGuests);
                return Ok(totalPrice);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while calculating the total price.");
            }
        }


        // hai3red el reservations dates
        //[Authorize(Roles = "client")]
        [HttpGet]
        [Route("Name/{Name:alpha}", Name = "GetByName")]
        public async Task<ActionResult<VenueDtoWithReservationData>> GetVenueByName(string Name)
        {
            try
            {
                List<VenueDtoWithReservationData> venues = await venueBLL.GetVenueByName(Name);
                return Ok(venues);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }

        }

        //[Authorize(Roles = "client")]
        [HttpGet]
        [Route("price/{price:int}", Name = "GetByPrice")]
        public async Task<ActionResult<VenueDtoWithReservationData>> GetVenueByPrice(double price)
        {
            try
            {
                List<VenueDtoWithReservationData> venues = await venueBLL.GetVenueByPrice(price);
                return Ok(venues);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }

        }



        //[Authorize(Roles = "client")]
        [HttpGet]
        [Route("{Location:alpha}")]
        public async Task<ActionResult<VenueDtoWithReservationData>> GetVenueByLocation(string Location)
        {
            try
            {
                List<VenueDtoWithReservationData> venues = await venueBLL.GetVenueByLocation(Location);
                return Ok(venues);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }


        //[Authorize(Roles = "ServiceProvider")]
        [HttpPost]
        public async Task<IActionResult> SaveVenue(VenueDto VenueDto)
        {
            try
            {
                await venueBLL.SaveVenue(VenueDto);
                return Ok(new { message = "Venue saved successfully." }); // Return JSON object with success messagel
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }




        }




        //[Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Venue>> UpdateVenue(int id,VenueDto VenueDto)
        {
            try
            {
                Venue updatedVenue = await venueBLL.UpdateVenue(id, VenueDto);

                if (updatedVenue != null)
                {
                    return Ok(updatedVenue); // Return updated venue
                }
                else
                {
                    return NotFound(); // Venue with the specified ID not found
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message); // Invalid argument
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Invalid ID
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Internal server error
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveVenue(int id)
        {
            try
            {
                await venueBLL.RemoveVenue(id);
                return StatusCode(204, "Venue removed successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Invalid argument
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Internal server error
            }
        }

        // admin accept request
        //[Authorize(Roles = "Admin")]
        [HttpPut("accept/{id}")]
        public async Task<IActionResult> AcceptVenueSubmission(int id)
        {
            // Call the business logic layer method
            bool accepted = await venueBLL.AcceptVenueSubmissionAsync(id);
            if (accepted)
            {
                return Ok("Venue submission accepted successfully.");
            }
            else
            {
                return NotFound(); // Venue not found
            }
        }

        //amin reject l request
        //[Authorize(Roles = "Admin")]
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectVenueSubmission(int id)
        {
            // Call the business logic layer method
            bool rejected = await venueBLL.RejectVenueSubmission(id);
            if (rejected)
            {
                return Ok("Venue submission rejected successfully.");
            }
            else
            {
                return NotFound(); // Venue not found
            }
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("ReservationUsers/{id}")]
        public async Task<ActionResult<VenueWithReservationUserDto>> GetByIdWithUsers(int id)
        {
            try
            {
                VenueWithReservationUserDto venueDto = await venueBLL.GetVenueByIdwithusers(id);
                return Ok(venueDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }


    }
}

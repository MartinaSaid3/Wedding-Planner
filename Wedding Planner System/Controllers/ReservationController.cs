using Data_Access_Layer.Repo.ReservationRepo;
using Business_Logic_Layer.Service.ReservationService;
using Business_Logic_Layer.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business_Logic_Layer.Dtos.ReservationDtos;
using Data_Access_Layer.Models;

using Business_Logic_Layer.Service.EmailService;
using Hangfire;

using System;
using Business_Logic_Layer.Service.VenueService;
using Data_Access_Layer.Repo.VenueRepo;
using Microsoft.AspNetCore.Authorization;


namespace Wedding_Planner_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private Business_Logic_Layer.Service.ReservationService.IReservationBLL ReservationBLL;
        private readonly IEmailSender emailsernder;

        public ReservationController(Business_Logic_Layer.Service.ReservationService.IReservationBLL _reservationBLL,IEmailSender _emailsernder)
        {
            ReservationBLL = _reservationBLL;
            emailsernder = _emailsernder;
        }
        [Authorize(Roles = "Admin")]
        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationWithTotalPriceDto>>> GetAllReservations()
        {
            var reservations = await ReservationBLL.GetAllReservations();
            return Ok(reservations);
        }

        [Authorize(Roles = "Admin")]
        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationWithTotalPriceDto>> GetReservation(int id)
        {
            var reservation = await ReservationBLL.GetReservation(id);

            return Ok(reservation);
        }

        //[Authorize(Roles = "client")]
        // POST: api/Reservations
        [HttpPost]
        public async Task<IActionResult> CreateReservation(ReservationDto reservationDto)
        {
            

            try
            {
                // Calculate total price
                double totalPrice = await ReservationBLL.CalculateTotalPrice(reservationDto.VenueId, reservationDto.NumOfGuests, reservationDto.Service);

                // Create reservation
                await ReservationBLL.CreateReservation(reservationDto);

                //background service
                RecurringJob.AddOrUpdate("back-recurring-job", () => ReservationBLL.Back(), Cron.Hourly(24));
                
                BackgroundJob.Enqueue(() => emailsernder.SendEmail("Successful Reservation", reservationDto.Email, reservationDto.Email, "", "Congratulation Your Registration Done Successfully"));


                // Schedule a rating reminder for the customer
                RecurringJob.AddOrUpdate("Rate-recurring-job", () => ReservationBLL.Rate(), Cron.Hourly(24));

                // Send "Thank you" email to the customer
                BackgroundJob.Enqueue(() => emailsernder.SendEmail("Congratulations",
                                                                    reservationDto.Email,
                                                                    reservationDto.Email,
                                                                    "",
                                                                    "Thank you for choosing us! We're thrilled to have hosted you."));

                return Ok();
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
                return StatusCode(500, "An error occurred while creating the reservation.");
            }



            




        }

        [Authorize(Roles = "client")]
        // PUT: api/Reservations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, ReservationDto reservationDTO)
        {
            Reservation reservation = await ReservationBLL.GetReservationForEdit(id);
 
            //// Check if there is already a reservation on the same day
            //if (await ReservationBLL.ReservationExistsForDate(reservationDTO.Date))
            //{
            //    return BadRequest("A reservation already exists for the selected date.");
            //}

            try
            {
                ReservationBLL.PutReservation(id, reservationDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ReservationBLL.ReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            await ReservationBLL.DeleteReservation(id);

            return NoContent();
        }

        [Authorize(Roles = "ServiceProvider")]

        [HttpPut("Accept/{id}")]
        public async Task<IActionResult> AcceptReservation(int id)
        {
            // Call the business logic layer method
            bool accepted = await ReservationBLL.AcceptReservation(id);
            if (accepted)
            {
                return Ok();
            }
            else
            {
                return NotFound(); // reservation not found
            }
        }

        [Authorize(Roles = "ServiceProvider")]
        [HttpPut("Reject/{id}")]
        public async Task<IActionResult> RejectReservation(int id)
        {
            // Call the business logic layer method
            bool rejected = await ReservationBLL.RejectReservationSubmission(id);
            if (rejected)
            {
                return Ok();
            }
            else
            {
                return NotFound(); // reservation not found
            }
        }



    }
}

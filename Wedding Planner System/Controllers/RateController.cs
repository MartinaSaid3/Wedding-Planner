using Business_Logic_Layer.Dtos.RatingDtos;
using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wedding_Planner_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly ApplicationEntity Context;

        public RateController(ApplicationEntity _context)
        {
            Context = _context;
        }


        [HttpPost]
        public async Task<IActionResult> SubmitReview(RatingDto reviewDto)
        {
            // Check if the reviewDto is null or invalid
            if (reviewDto == null)
            {
                return BadRequest("Invalid review data.");
            }

            // Authenticate user (implement authentication logic here)

            // Check if the reservation associated with the review exists
            var reservation = await Context.Reservations.FindAsync(reviewDto.ReservationId);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            // Check if the current date is after the reservation date
            if (DateTime.Today < reservation.Date)
            {
                return BadRequest("You can only leave a review after your reservation date has passed.");
            }

            // Create a new Review entity and populate it with data from the DTO
            Rate review = new Rate
            {
                ReservationId = reviewDto.ReservationId,
                Rating = reviewDto.Rating,
                Comment = reviewDto.ReviewText,

            };

            // Save the review to the database
            Context.Rates.Add(review);
            await Context.SaveChangesAsync();

            // Return a success response
            return Ok("Review submitted successfully.");
        }

    }
}

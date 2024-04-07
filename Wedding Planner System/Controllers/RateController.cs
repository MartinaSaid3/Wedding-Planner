using Business_Logic_Layer.Dtos.RatingDtos;
using Business_Logic_Layer.Service.RateService;
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
        private readonly IRateBLL rateBLL;

        public RateController(IRateBLL _rateBLL)
        {
            rateBLL = _rateBLL;
        }


        [HttpPost("submit")]
        public async Task<IActionResult> SubmitRating(RatingDto ratingDto)
        {
            // Validate ratingValue
            if (ratingDto.Rating < 1 || ratingDto.Rating > 5)
            {
                return BadRequest("Invalid rating value. Rating must be between 1 and 5.");
            }

            // Check if the user has already submitted a rating for this venue
            var existingRating = await rateBLL.GetRatingByVenueAndUser(ratingDto.VenueId, ratingDto.UserId);
            if (existingRating != null)
            {
                return BadRequest("You have already submitted a rating for this venue.");
            }

            // Save rating to database
            var rating = new Rate
            {
                VenueId = ratingDto.VenueId,
                UserId = ratingDto.UserId,
                Rating = ratingDto.Rating
            };

            await rateBLL.AddRating(rating);


            return Ok("Rating submitted successfully.");


        }

    }
}

using Data_Access_Layer.Repo.ReservationRepo;
using Business_Logic_Layer.Service.ReservationService;
using Business_Logic_Layer.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business_Logic_Layer.Dtos.ReservationDtos;
using Data_Access_Layer.Models;

namespace Wedding_Planner_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private Business_Logic_Layer.Service.ReservationService.IReservationBLL ReservationBLL;

        public ReservationController(Business_Logic_Layer.Service.ReservationService.IReservationBLL _reservationBLL)
        {
            ReservationBLL = _reservationBLL;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAllReservations()
        {
            var reservations = await ReservationBLL.GetAllReservations();
            return Ok(reservations);
        }

        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetReservation(int id)
        {
            var reservation = await ReservationBLL.GetReservation(id);

            return Ok(reservation);
        }

        // POST: api/Reservations
        [HttpPost]
        public async Task<IActionResult> CreateReservation(ReservationDto reservationDto)
        {
            //// Check if there is already a reservation on the same day
            //if (await ReservationBLL.ReservationExistsForDate(reservationDto.Date))
            //{
            //    return BadRequest("A reservation already exists for the selected date.");
            //}
            await ReservationBLL.CreateReservation(reservationDto);
            return Ok("A New Reservation Added Successfully!");

            //return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservationDto);
        }


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

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            await ReservationBLL.DeleteReservation(id);

            return NoContent();
        }

    }
}

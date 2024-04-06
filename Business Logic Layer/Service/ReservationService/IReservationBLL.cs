using Business_Logic_Layer.Dtos.ReservationDtos;
using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.ReservationService
{
    public interface IReservationBLL
    {
        Task<List<ReservationWithTotalPriceDto>> GetAllReservations();
        Task<ReservationWithTotalPriceDto> GetReservation(int id);
        Task<ServicesResult<ApplicationUser>> CreateReservation(ReservationDto reservationDto);
        Task<bool> IsDateAvailable(DateTime date, int venueId);
        Task<bool> ReservationExistsForDate(DateTime date);
        Task<ServicesResult<ApplicationUser>> PutReservation(int id, ReservationDto reservationDTO);
        Task<Reservation> GetReservationForEdit(int id);
        Task<bool> ReservationExists(int id);
        Task<Reservation> DeleteReservation(int id);
        Task<double> CalculateTotalPrice(int venueId, int numOfGuests, string selectedService);


        string GenerateUniqueToken();
        Task Back();
        Task Rate();
      

        Task<bool> AcceptReservation(int id);
        Task<bool> RejectReservationSubmission(int id);

    }
}

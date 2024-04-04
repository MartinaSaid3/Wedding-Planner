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
        Task<List<ReservationDto>> GetAllReservations();
        Task<ReservationDto> GetReservation(int id);
        void CreateReservation(ReservationDto reservationDto);
        Task<bool> IsDateAvailable(DateTime date, int venueId);
        Task<bool> ReservationExistsForDate(DateTime date);
        void PutReservation(int id, ReservationDto reservationDTO);
        Task<Reservation> GetReservationForEdit(int id);
        Task<bool> ReservationExists(int id);
        Task<Reservation> DeleteReservation(int id);
    }
}

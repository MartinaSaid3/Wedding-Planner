using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Repo.ReservationRepo
{
    public interface IReservationDAL
    {
        Task<List<Reservation>> GetAllReservations();
        Task<Reservation> GetReservation(int id);
        Task CreateReservation(Reservation reservation);
        Task<bool> IsDateAvailable(DateTime date, int venueId);
        Task<bool> ReservationExistsForDate(DateTime date);
        Task PutReservation(int id, Reservation reservation);
        Task<bool> ReservationExists(int id);
        Task<Reservation> DeleteReservation(int id);
        Task<List<Reservation>> GetReservationsThreeDaysFromNow();

        Task<List<Reservation>> GetReservationsForFeedback(DateTime yesterday);

        Task saveChanges();

    }
}

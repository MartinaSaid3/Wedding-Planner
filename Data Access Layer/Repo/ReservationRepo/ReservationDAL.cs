using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repo.ReservationRepo
{
    public class ReservationDAL : IReservationDAL
    {
        private readonly ApplicationEntity context;
        //private readonly ApplicationEntity _context;
        public ReservationDAL(ApplicationEntity _context)
        {
            context = _context;
        }
        public async Task<List<Reservation>> GetAllReservations()
        {
            var reservations = await context.Reservations.ToListAsync();

            return reservations;
        }
        public async Task<Reservation> GetReservation(int id)
        {
            Reservation reservation = await context.Reservations.Include(s => s.Venue).FirstOrDefaultAsync(r => r.Id == id);

            return reservation;
        }
        public async Task CreateReservation(Reservation reservation)
        {
            context.Reservations.Add(reservation);
            await context.SaveChangesAsync();
        }
        public async Task<bool> IsDateAvailable(DateTime date, int venueId)
        {
            // Check if there are any existing reservations for the given date and venue
            return !await context.Reservations.AnyAsync(r => r.Date.Date == date.Date && r.VenueId == venueId);
        }

        public async Task<bool> ReservationExistsForDate(DateTime date)
        {
            return await context.Reservations.AnyAsync(r => r.Date.Date == date.Date);
        }
        public async Task PutReservation(int id, Reservation reservation)
        {
            reservation = await context.Reservations.Include(s => s.Venue).FirstOrDefaultAsync(r => r.Id == id);


            context.Entry(reservation).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }
        public async Task<bool> ReservationExists(int id)
        {
            return !await context.Reservations.AnyAsync(e => e.Id == id);
        }
        public async Task<Reservation> DeleteReservation(int id)
        {
            var reservation = await context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return null;
            }

            context.Reservations.Remove(reservation);
            await context.SaveChangesAsync();

            return reservation;
        }


        public async Task < List<Reservation>> GetReservationsThreeDaysFromNow()
        {
            DateTime targetDate = DateTime.Now.AddDays(3);
            return await context.Reservations.Where(x => x.Date.Date == targetDate.Date).ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsForFeedback(DateTime yesterday)
        {
            return await context.Reservations.Where(x => x.Date.Date == yesterday.Date).ToListAsync();
        }


        public async Task saveChanges()
        {
            await context.SaveChangesAsync();

        }
    }
}

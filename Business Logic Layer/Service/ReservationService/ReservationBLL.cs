using AutoMapper;
using Business_Logic_Layer.Dtos.ReservationDtos;
using Business_Logic_Layer.Service.EmailService;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repo.ReservationRepo;
using Hangfire;

namespace Business_Logic_Layer.Service.ReservationService
{
    public class ReservationBLL : IReservationBLL
    {
        private Data_Access_Layer.Repo.ReservationRepo.IReservationDAL ReservationDAL;
        private Mapper PersonMapper;
        public ReservationBLL(Data_Access_Layer.Repo.ReservationRepo.IReservationDAL _ReservationDAL)
        {
            ReservationDAL = _ReservationDAL;
            var configPeron = new MapperConfiguration(cfg => cfg.CreateMap<Reservation, ReservationDto>().ReverseMap());
            PersonMapper = new Mapper(configPeron);
        }
        public async Task<List<ReservationDto>> GetAllReservations()
        {
            List<Reservation> reservationsFromDataBase = await ReservationDAL.GetAllReservations();
            var myData = reservationsFromDataBase.ToList();
            var reservationDtos = myData.Select(x => new ReservationDto
            {
                Date = x.Date,
                Email = x.Email,
                NumOfGuests = x.NumOfGuests,
                SpecialRequests = x.SpecialRequests,
                VenueId = x.VenueId

            }).ToList();
            return reservationDtos;
        }
        public async Task<ReservationDto> GetReservation(int id)
        {
            var reservation = await ReservationDAL.GetReservation(id);

            ReservationDto reservationDto = PersonMapper.Map<Reservation, ReservationDto>(reservation);

            return reservationDto;
        }
        public async Task CreateReservation(ReservationDto reservationDto)
        {
            // Check if the date is available for reservation
            if (!await ReservationDAL.IsDateAvailable(reservationDto.Date, reservationDto.VenueId))
            {
                throw new InvalidOperationException("The date is not available for reservation.");
            }
            Reservation reservation = PersonMapper.Map<ReservationDto, Reservation>(reservationDto);

            await ReservationDAL.CreateReservation(reservation);
        }
        public async Task<bool> IsDateAvailable(DateTime date, int venueId)
        {
            return await ReservationDAL.IsDateAvailable(date, venueId);
        }

        public async Task<bool> ReservationExistsForDate(DateTime date)
        {
            return await ReservationDAL.ReservationExistsForDate(date);
        }
        public async Task PutReservation(int id, ReservationDto reservationDTO)
        {
            // Check if the date is available for reservation
            if (!await ReservationDAL.IsDateAvailable(reservationDTO.Date, reservationDTO.VenueId))
            {
                throw new InvalidOperationException("The date is not available for reservation.");
            }

            Reservation reservation = PersonMapper.Map<ReservationDto, Reservation>(reservationDTO);
            await ReservationDAL.PutReservation(id, reservation);

        }
        public async Task<Reservation> GetReservationForEdit(int id)
        {
            var reservation = await ReservationDAL.GetReservation(id);
            if (id != reservation.Id)
            {
                throw new InvalidOperationException();
            }

            //var reservation = await context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                throw new InvalidOperationException();
            }

            return reservation;
        }
        public async Task<bool> ReservationExists(int id)
        {
            return await ReservationDAL.ReservationExists(id);
        }
        public async Task<Reservation> DeleteReservation(int id)
        {
            var reservation = await ReservationDAL.DeleteReservation(id);
            if (reservation == null)
            {
                throw new InvalidOperationException();
            }
            return reservation;
        }

        // Generate a unique token for the reservation
        public string GenerateUniqueToken()
        {
           
            return Guid.NewGuid().ToString();
        }


        //function to remind date of event 3 days left
        public async Task Back()
        {
            List<Reservation> reservations = await ReservationDAL.GetReservationsThreeDaysFromNow();
            foreach (var reservation in reservations)
            {
                EmailSender email = new EmailSender();
                BackgroundJob.Enqueue(() => email.
                SendEmail("Reminder To You", reservation.Email,
                " Client", $" , Congratulation, Your Wedding Party Will be After 3 Days From Now", ""));
            }


        }


        public async Task Rate()
        {
            List<Reservation> reservations = await ReservationDAL.GetReservationsForFeedback();
            foreach (var reservation in reservations)
            {
                EmailSender email = new EmailSender();
                BackgroundJob.Enqueue(() => email.
                SendEmail("Feedback", reservation.Email,
                "Client", $"Dear Client,\n\n" +
                $"Thank you for choosing our venue. We hope you enjoyed your experience with us." +
                $"\n\nPlease take a moment to rate and review our venue by clicking on the link below" +
                $"\nYour feedback is valuable to us!\n\nBest regards,\nYour Venue Team", "<a href='{ratingReviewUrl}'>Rate and Review</a>"));
            }



        }
    }
}

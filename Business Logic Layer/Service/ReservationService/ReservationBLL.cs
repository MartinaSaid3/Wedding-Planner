using AutoMapper;
using Business_Logic_Layer.Dtos.ReservationDtos;
using Business_Logic_Layer.Service.EmailService;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repo.ReservationRepo;

using Hangfire;

using Data_Access_Layer.Repo.VenueRepo;


namespace Business_Logic_Layer.Service.ReservationService
{
    public class ReservationBLL : IReservationBLL
    {
        private Data_Access_Layer.Repo.ReservationRepo.IReservationDAL ReservationDAL;
        private readonly IVenueDAL venueDAL;
        private readonly IEmailSender _emailSender;
        private Mapper PersonMapper;
        public ReservationBLL(Data_Access_Layer.Repo.ReservationRepo.IReservationDAL _ReservationDAL,
            IVenueDAL _venueDAL,
            IEmailSender emailSender)
        {
            ReservationDAL = _ReservationDAL;
            venueDAL = _venueDAL;
            _emailSender = emailSender;
            var configPeron = new MapperConfiguration(cfg => cfg.CreateMap<Reservation, ReservationDto>().ReverseMap());
            PersonMapper = new Mapper(configPeron);
        }
        public async Task<List<ReservationWithTotalPriceDto>> GetAllReservations()
        {
            List<Reservation> reservationsFromDataBase = await ReservationDAL.GetAllReservations();
            var myData = reservationsFromDataBase.ToList();
            var reservationDtos = myData.Select(x => new ReservationWithTotalPriceDto
            {
                Id=x.Id,
                Date = x.Date,
                Email = x.Email,
                NumOfGuests = x.NumOfGuests,
                SpecialRequests = x.SpecialRequests,
                Service = x.Service,
                UserName = x.UserName,
                VenueId = x.VenueId,
                TotalPrice = x.TotalPrice


            }).ToList();
            return reservationDtos;
        }
        public async Task<ReservationWithTotalPriceDto> GetReservation(int id)
        {
            var reservation = await ReservationDAL.GetReservation(id);

            ReservationWithTotalPriceDto reservationDto = new ReservationWithTotalPriceDto
            {
                Id=reservation.Id,
                UserName = reservation.UserName,
                Date = reservation.Date,
                NumOfGuests = reservation.NumOfGuests,
                VenueId = reservation.VenueId,
                SpecialRequests = reservation.SpecialRequests,
                Email = reservation.Email,
                Service = reservation.Service,
                TotalPrice = reservation.TotalPrice
            };

            return reservationDto;
        }
        public async Task<ServicesResult<ApplicationUser>> CreateReservation(ReservationDto reservationDto)
        {
            // Check if the date is available for reservation
            if (!await ReservationDAL.IsDateAvailable(reservationDto.Date, reservationDto.VenueId))
            {
                return ServicesResult<ApplicationUser>.Failure("The date is not available for reservation.");
            }

            // Calculate total price based on venue, number of guests, and selected service
            double totalPrice = await CalculateTotalPrice(reservationDto.VenueId, reservationDto.NumOfGuests, reservationDto.Service);

            Reservation reservation = PersonMapper.Map<ReservationDto, Reservation>(reservationDto);
            reservation.TotalPrice = totalPrice;

            await ReservationDAL.CreateReservation(reservation);
            return ServicesResult<ApplicationUser>.Successed(default, "Added Successfully");
        }
        public async Task<bool> IsDateAvailable(DateTime date, int venueId)
        {
            return await ReservationDAL.IsDateAvailable(date, venueId);
        }

        public async Task<bool> ReservationExistsForDate(DateTime date)
        {
            return await ReservationDAL.ReservationExistsForDate(date);
        }
        public async Task<ServicesResult<ApplicationUser>> PutReservation(int id, ReservationDto reservationDTO)
        {
            // Check if the date is available for reservation
            if (!await ReservationDAL.IsDateAvailable(reservationDTO.Date, reservationDTO.VenueId))
            {
                return ServicesResult<ApplicationUser>.Failure("The date is not available for reservation.");
            }

            Reservation reservation = PersonMapper.Map<ReservationDto, Reservation>(reservationDTO);
            await ReservationDAL.PutReservation(id, reservation);
            return ServicesResult<ApplicationUser>.Successed(default, "Updated Successfully");

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
                BackgroundJob.Enqueue(() => _emailSender.
                SendEmail("Reminder To You", reservation.Email,
                " Client", "", $" , Congratulation, Your Wedding Party Will be After 3 Days From Now"));
            }
        }


        public async Task Rate()
        {
            DateTime yesterday = DateTime.Now.Date.AddDays(-1); // Get yesterday's date
            List<Reservation> reservations = await ReservationDAL.GetReservationsForFeedback(yesterday);

            foreach (var reservation in reservations)
            {
                // Replace {ratingReviewUrl} with the actual URL for rating and reviewing the venue
                string ratingReviewUrl = "https://example.com/rate-and-review";

                // Send email reminder for rating
                BackgroundJob.Enqueue(() => _emailSender.SendEmail("Feedback",
                                                                    reservation.Email,
                                                                    "Client",
                                                                    $"<a href='{ratingReviewUrl}'>Rate and Review</a>",
                                                                    $"Dear Client,\n\n" +
                                                                    $"Thank you for choosing our venue. We hope you enjoyed your experience with us.\n\n" +
                                                                    $"Please take a moment to rate and review our venue by clicking on the link below.\n\n" +
                                                                    $"Your feedback is valuable to us!\n\n" +
                                                                    $"Best regards,\nYour Venue Team"));
            }
        }



        public async Task<bool> AcceptReservation(int id)
        {
            var reservation = await ReservationDAL.GetReservation(id);
            if (reservation == null)
            {
                return false; // reservation not found
            }

            // Update the status of the reservation submission to Accepted
            reservation.Status = Reservation.ApprovalStatusReservation.Accepted;
            await ReservationDAL.saveChanges();

            return true; // Venue submission accepted successfully
        }

        public async Task<bool> RejectReservationSubmission(int id)
        {
            var reservation = await ReservationDAL.GetReservation(id);
            if (reservation == null)
            {
                return false;
            }

            // Update the status of the venue submission to Rejected
            reservation.Status = Reservation.ApprovalStatusReservation.Rejected;
            await ReservationDAL.saveChanges();

            return true;

        }

        public async Task<double> CalculateTotalPrice(int venueId, int numOfGuests, string selectedService)
        {
            // Get venue details
            Venue venue = await venueDAL.GetVenueById(venueId);

            if (venue == null)
            {
                throw new ArgumentNullException("Venue not found.");
            }

            // Trim and convert selectedService to lower case for case-insensitive comparison
            selectedService = selectedService.Trim().ToLower();

            // Calculate total price based on the selected service
            double totalPrice = 0;
            switch (selectedService)
            {
                case "openbuffet":
                    totalPrice = numOfGuests * venue.PriceOpenBuffetPerPerson;
                    break;
                case "setmenue":
                    totalPrice = numOfGuests * venue.PriceSetMenuePerPerson;
                    break;
                case "hightea":
                    totalPrice = numOfGuests * venue.PriceHighTeaPerPerson;
                    break;
                default:
                    throw new ArgumentException("Invalid service selected.");
            }

            return totalPrice;
        }
    }
}

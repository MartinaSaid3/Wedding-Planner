using AutoMapper;
using Business_Logic_Layer.Dtos.ReservationDtos;
using Data_Access_Layer.Models;

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
        public async void CreateReservation(ReservationDto reservationDto)
        {
            Reservation reservation = PersonMapper.Map<ReservationDto, Reservation>(reservationDto);

            ReservationDAL.CreateReservation(reservation);
        }
        public async Task<bool> IsDateAvailable(DateTime date, int venueId)
        {
            return await ReservationDAL.IsDateAvailable(date, venueId);
        }

        public async Task<bool> ReservationExistsForDate(DateTime date)
        {
            return await ReservationDAL.ReservationExistsForDate(date);
        }
        public async void PutReservation(int id, ReservationDto reservationDTO)
        {

            Reservation reservation = PersonMapper.Map<ReservationDto, Reservation>(reservationDTO);
            ReservationDAL.PutReservation(id, reservation);

        }
        public async Task<Reservation> GetReservationForEdit(int id)
        {
            var reservation = await ReservationDAL.GetReservation(id);

            return reservation;
        }
        public async Task<bool> ReservationExists(int id)
        {
            return await ReservationDAL.ReservationExists(id);
        }
        public async Task<Reservation> DeleteReservation(int id)
        {
            return await ReservationDAL.DeleteReservation(id);
        }
    }
}

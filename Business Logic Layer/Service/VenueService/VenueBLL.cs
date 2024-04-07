using Business_Logic_Layer.Dtos.VenueDtos;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repo.RateRepo;
using Data_Access_Layer.Repo.VenueRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.VenueService
{
    public class VenueBLL : IVenueBLL
    {
        private readonly IVenueDAL venueDAL;
        private readonly IRateDAL _rateDAL;
        private readonly IPhotoServices photoservices;

        public VenueBLL(IVenueDAL _VenueDAL,
            IRateDAL rateDAL,
            IPhotoServices _photoservices)
        {
            venueDAL = _VenueDAL;
            _rateDAL = rateDAL;
            photoservices = _photoservices;
        }
        public async Task<List<VenueWithReservationIdDto>> GetAllVenue()
        {

            List<Venue> VenueList = await venueDAL.GetAllVenues();

            if (VenueList.Count > 0)
            {
                List<VenueWithReservationIdDto> ListOfVenueDto = new List<VenueWithReservationIdDto>();
                foreach (var item in VenueList)
                {

                    VenueWithReservationIdDto venueDto = new VenueWithReservationIdDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Location = item.Location,
                        OpenBuffet = item.OpenBuffet,
                        SetMenue = item.SetMenue,
                        HighTea = item.HighTea,
                        MaxCapacity = item.MaxCapacity,
                        MinCapacity = item.MinCapacity,
                        PriceStartingFrom = item.MinPrice,
                        ImagesData = item.ImagesData

                    };

                    ListOfVenueDto.Add(venueDto);
                }
                return ListOfVenueDto;

            }
            throw new Exception("There are no halls");

        }


        public async Task<VenueDtoWithReservationData> GetVenueById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid venue ID. Please provide a valid ID.");
            }

            Venue v1 = await venueDAL.GetVenueById(id);

            if (v1 == null)
            {
                throw new Exception("This Venue Does Not Exist");
            }

            var averageRate = await _rateDAL.GetAverageRateForVenueOrDefaultAsync(id);
            VenueDtoWithReservationData venueDto = new VenueDtoWithReservationData
            {
                Id = v1.Id,
                Name = v1.Name,
                Description = v1.Description,
                Location = v1.Location,
                OpenBuffet = v1.OpenBuffet,
                SetMenue = v1.SetMenue,
                HighTea = v1.HighTea,
                PriceStartingFrom = v1.MinPrice,
                MinCapacity = v1.MinCapacity,
                MaxCapacity = v1.MaxCapacity,
                ImagesData = v1.ImagesData,
                ReservationDates = v1.Reservations.Select(r => r.Date).ToList(),
                AverageRate = averageRate
            };

            return venueDto;
        }

        public async Task<List<VenueDtoWithReservationData>> GetVenueByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentException("You must enter a name.");
            }

            List<Venue> venueList = await venueDAL.GetVenueByName(name);

            if (venueList.Count == 0)
            {
                throw new InvalidOperationException("There are no venues with the same name.");
            }

            List<VenueDtoWithReservationData> listVenueDTO = new List<VenueDtoWithReservationData>();

            foreach (var item in venueList)
            {
                VenueDtoWithReservationData venueDto = new VenueDtoWithReservationData
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Location = item.Location,
                    OpenBuffet = item.OpenBuffet,
                    SetMenue = item.SetMenue,
                    HighTea = item.HighTea,
                    MaxCapacity = item.MaxCapacity,
                    MinCapacity = item.MinCapacity,
                    PriceStartingFrom = item.MinPrice,
                    ImagesData = item.ImagesData,
                    ReservationDates = item.Reservations.Select(r => r.Date).ToList()
                };

                listVenueDTO.Add(venueDto);
            }

            return listVenueDTO;
        }

        public async Task<List<VenueDtoWithReservationData>> GetVenueByPrice(double price)
        {
            if (price == null)
            {
                throw new ArgumentException("You must enter price");
            }

            List<Venue> VenueList = await venueDAL.GetVenueByPrice(price);

            if (VenueList.Count == 0)
            {
                throw new InvalidOperationException("There are no venues with the same price");
            }

            List<VenueDtoWithReservationData> ListVenueDTO = new List<VenueDtoWithReservationData>();

            foreach (var item in VenueList)
            {
                VenueDtoWithReservationData VenueDto = new VenueDtoWithReservationData
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Location = item.Location,
                    OpenBuffet = item.OpenBuffet,
                    SetMenue = item.SetMenue,
                    HighTea = item.HighTea,
                    MaxCapacity = item.MaxCapacity,
                    MinCapacity = item.MinCapacity,
                    PriceStartingFrom = item.MinPrice,
                    ImagesData = item.ImagesData,
                    ReservationDates = new List<DateTime>() // Initialize the list
                };

                foreach (var reservation in item.Reservations)
                {
                    VenueDto.ReservationDates.Add(reservation.Date);
                }
                ListVenueDTO.Add(VenueDto);
            }

            return ListVenueDTO;

        }


        public async Task<List<VenueDtoWithReservationData>> GetVenueByLocation(string Location)
        {


            if (string.IsNullOrWhiteSpace(Location))
            {
                throw new ArgumentException("You must enter a location");
            }

            List<Venue> VenueList = await venueDAL.GetVenueByLocation(Location);

            if (VenueList == null || VenueList.Count == 0)
            {
                throw new InvalidOperationException("There are no Venue with the same city");
            }

            List<VenueDtoWithReservationData> ListVenueDTO = new List<VenueDtoWithReservationData>();

            foreach (var item in VenueList)
            {
                VenueDtoWithReservationData VenueDto = new VenueDtoWithReservationData
                {

                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Location = item.Location,
                    OpenBuffet = item.OpenBuffet,
                    SetMenue = item.SetMenue,
                    HighTea = item.HighTea,
                    MaxCapacity = item.MaxCapacity,
                    MinCapacity = item.MinCapacity,
                    PriceStartingFrom = item.MinPrice,
                    ImagesData = item.ImagesData,

                    ReservationDates = new List<DateTime>() // Initialize the list
                };


                foreach (var reservation in item.Reservations)
                {
                    VenueDto.ReservationDates.Add(reservation.Date);
                }
                ListVenueDTO.Add(VenueDto);
            }

            return ListVenueDTO;

        }


        public async Task SaveVenue(VenueDto VenueDto)
        {

            List<string> imagesUrl = new List<string>();

            foreach (var x in VenueDto.ImagesData)
            {

                var photo = await photoservices.AddPlaceAsync(x);
                imagesUrl.Add(photo.SecureUrl.AbsoluteUri);
            }


            var venue = new Venue
            {
                Name = VenueDto.Name,
                Description = VenueDto.Description,
                Location = VenueDto.Location,
                OpenBuffet = VenueDto.OpenBuffet,
                SetMenue = VenueDto.SetMenue,
                HighTea = VenueDto.HighTea,
                PriceOpenBuffetPerPerson = VenueDto.PriceOpenBuffetPerPerson,
                PriceSetMenuePerPerson = VenueDto.PriceSetMenuePerPerson,
                PriceHighTeaPerPerson = VenueDto.PriceHighTeaPerPerson,
                MinCapacity = VenueDto.MinCapacity,
                MaxCapacity = VenueDto.MaxCapacity,
                //conert image to url
                ImagesData = imagesUrl,
            };

            // save the venue to the database

            await venueDAL.SaveVenue(venue);


        }



        public async Task<List<byte[]>> ConvertImagesToByteArray(List<IFormFile> imageFiles)
        {
            var imageDataList = new List<byte[]>();

            foreach (var imageFile in imageFiles)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        imageDataList.Add(memoryStream.ToArray());
                    }
                }
            }

            return imageDataList;
        }

        public async Task<Venue> UpdateVenue(int id, VenueDto venueDto)
        {
            // Validate DTO
            if (venueDto == null)
            {
                throw new ArgumentNullException(nameof(venueDto), "Venue DTO is null.");
            }

            // Get the old venue from the database
            Venue oldVenue = await venueDAL.GetVenueById(id);

            if (oldVenue == null)
            {
                throw new ArgumentException("Invalid venue ID.", nameof(id));
            }

            // Update the old venue with the new data
            oldVenue.Name = venueDto.Name;
            oldVenue.Description = venueDto.Description;
            oldVenue.Location = venueDto.Location;
            oldVenue.OpenBuffet = venueDto.OpenBuffet;
            oldVenue.SetMenue = venueDto.SetMenue;
            oldVenue.HighTea = venueDto.HighTea;
            oldVenue.PriceOpenBuffetPerPerson = venueDto.PriceOpenBuffetPerPerson;
            oldVenue.PriceSetMenuePerPerson = venueDto.PriceSetMenuePerPerson;
            oldVenue.PriceHighTeaPerPerson = venueDto.PriceHighTeaPerPerson;
            oldVenue.MaxCapacity = venueDto.MaxCapacity;
            oldVenue.MinCapacity = venueDto.MinCapacity;
            //oldVenue.ImagesData = venueDto.ImagesData;

            // Update the venue in the database
            await venueDAL.UpdateVenueAsync(oldVenue);

            return oldVenue;
        }

        public async Task RemoveVenue(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid venue ID. Please provide a valid ID.", nameof(id));
            }

            Venue venue = await venueDAL.GetVenueById(id);

            if (venue == null)
            {
                throw new ArgumentException("Venue not found with the same ID.", nameof(id));
            }

            await venueDAL.RemoveVenue(venue);
        }


        public async Task<double> CalculateTotalPrice(int venueId, string selectedService, int numberOfGuests)
        {

            Venue venue = await venueDAL.GetVenueById(venueId);

            if (venue == null)
            {
                throw new ArgumentNullException("Venue not found.");
            }

            double totalPrice = 0;

            // Trim and convert selectedService to lower case for case-insensitive comparison
            selectedService = selectedService.Trim().ToLower();

            // Validate the selected service
            switch (selectedService)
            {
                case "openbuffet":
                    totalPrice = numberOfGuests * venue.PriceOpenBuffetPerPerson;
                    break;
                case "setmenue":
                    totalPrice = numberOfGuests * venue.PriceSetMenuePerPerson;
                    break;
                case "hightea":
                    totalPrice = numberOfGuests * venue.PriceHighTeaPerPerson;
                    break;
                default:
                    throw new ArgumentException("Invalid service selected.");
            }

            //venue.TotalPrice = totalPrice;
            //await Context.Venues.AddAsync(venue);
            //await Context.SaveChangesAsync();

            return totalPrice;
        }

        public async Task<bool> AcceptVenueSubmissionAsync(int id)
        {
            var venue = await venueDAL.GetVenueById(id);
            if (venue == null)
            {
                return false; // Venue not found
            }

            // Update the status of the venue submission to Accepted
            venue.Status = Venue.ApprovalStatus.Accepted;
            await venueDAL.saveChanges();

            return true; // Venue submission accepted successfully
        }

        public async Task<bool> RejectVenueSubmission(int id)
        {
            var venue = await venueDAL.GetVenueById(id);
            if (venue == null)
            {
                return false;
            }

            // Update the status of the venue submission to Rejected
            venue.Status = Venue.ApprovalStatus.Rejected;
            await venueDAL.saveChanges();

            return true;
        }

        public async Task<VenueWithReservationUserDto> GetVenueByIdwithusers(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid venue ID. Please provide a valid ID.");
            }

            Venue v1 = await venueDAL.GetVenueById(id);

            if (v1 == null)
            {
                throw new Exception("This Venue Does Not Exist");
            }

            VenueWithReservationUserDto venueDto = new VenueWithReservationUserDto
            {
                Id = v1.Id,
                Name = v1.Name,
                Description = v1.Description,
                Location = v1.Location,
                OpenBuffet = v1.OpenBuffet,
                SetMenue = v1.SetMenue,
                HighTea = v1.HighTea,
                PriceStartingFrom = v1.MinPrice,
                MinCapacity = v1.MinCapacity,
                MaxCapacity = v1.MaxCapacity,
                ImagesData = v1.ImagesData,
                ReservationUser = v1.Reservations.Select(r => r.UserName).ToList()

            };

            return venueDto;

        }



    }
}

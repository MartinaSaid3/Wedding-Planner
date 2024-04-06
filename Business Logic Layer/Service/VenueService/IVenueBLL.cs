using Business_Logic_Layer.Dtos.VenueDtos;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.VenueService
{
    public interface IVenueBLL
    {
        Task<List<VenueWithReservationIdDto>> GetAllVenue();
        Task<VenueDtoWithReservationData> GetVenueById(int id);
        Task<VenueWithReservationUserDto> GetVenueByIdwithusers(int id);
        Task<List<VenueDtoWithReservationData>> GetVenueByName(string name);
        Task<List<VenueDtoWithReservationData>> GetVenueByPrice(double price);
        Task<List<VenueDtoWithReservationData>> GetVenueByLocation(string Location);
        Task SaveVenue(VenueDto VenueDto);
        Task<List<byte[]>> ConvertImagesToByteArray(List<IFormFile> imageFiles);
        Task<Venue> UpdateVenue(int id, VenueDto venueDto);
        Task RemoveVenue(int id);
        Task<double> CalculateTotalPrice(int venueId, string selectedService, int numberOfGuests);

        Task<bool> AcceptVenueSubmissionAsync(int id);
        Task<bool> RejectVenueSubmission(int id);



    }
}

using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Repo.VenueRepo
{
    public interface IVenueDAL
    {
        Task<List<Venue>> GetAllVenues();
        Task<Venue> GetVenueById(int id);
        Task<List<Venue>> GetVenueByName(string Name);
        Task<List<Venue>> GetVenueByPrice(double price);
        Task<List<Venue>> GetVenueByLocation(string Location);
        Task SaveVenue(Venue venue);
        Task UpdateVenueAsync(Venue venue);
        Task RemoveVenue(Venue venue);
        Task saveChanges();
    }
}

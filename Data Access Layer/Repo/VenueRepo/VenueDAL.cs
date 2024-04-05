using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Repo.VenueRepo
{
    public class VenueDAL :IVenueDAL
    {
        private readonly ApplicationEntity Context;

        public VenueDAL(ApplicationEntity _context)
        {
            Context = _context;
        }

        public async Task<List<Venue>> GetAllVenues()
        {
            return await Context.Venues
                .Include(a => a.Reservations)
                .ToListAsync();
        }

       
        public async Task<Venue> GetVenueById(int id)
        {
            return await Context.Venues.Include(a => a.Reservations)
                .FirstOrDefaultAsync(a => a.Id == id);
        }


        public async Task<List<Venue>> GetVenueByName(string Name)
        {
            return await Context.Venues.Include(v => v.Reservations)
            .Where(a => a.Name == Name).ToListAsync();
        }


        public async Task<List<Venue>> GetVenueByPrice(double price)
        {

            return await Context.Venues.Include(v => v.Reservations)
             .Where(a => a.MinPrice <= price).ToListAsync();

        }


        public async Task<List<Venue>> GetVenueByLocation(string Location)
        {

            return await Context.Venues.Include(v => v.Reservations)
               .Where(a => a.Location.Contains(Location)).ToListAsync();

        }

        public async Task SaveVenue(Venue venue)
        {
            Context.Venues.Add(venue);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateVenueAsync(Venue venue)
        {
            Context.Update(venue);
            await Context.SaveChangesAsync();
        }

        public async Task RemoveVenue(Venue venue)
        {

            Context.Venues.Remove(venue);
            Context.SaveChanges();

        }

        public async Task saveChanges()
        {
            Context.SaveChanges();
        }
    }
}

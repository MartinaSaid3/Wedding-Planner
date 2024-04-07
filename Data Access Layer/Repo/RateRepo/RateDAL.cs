
using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repo.RateRepo;

public class RateDAL : IRateDAL
{
    private readonly ApplicationEntity Context;

    public RateDAL(ApplicationEntity context)
    {
        Context = context;
    }

    public async Task AddRating(Rate rate)
    {
        Context.Rates.Add(rate);
        await Context.SaveChangesAsync();
    }

    public async Task<double> GetAverageRateForVenueOrDefaultAsync(int venueId)
    {
        var query = Context.Rates
             .Where(r => r.VenueId == venueId);

        if (query.Any())
        {
            return await query.AverageAsync(r => r.Rating);
        }

        return default;
    }

    public async Task<Rate> GetRatingByVenueAndUser(int venueId, int userId)
    {
        return await Context.Rates
            .FirstOrDefaultAsync(r => r.VenueId == venueId && r.UserId == userId);
    }

    public async Task<List<Rate>> GetRatingsForVenue(int venueId)
    {
        return await Context.Rates.Where(r => r.VenueId == venueId).ToListAsync();
    }
}

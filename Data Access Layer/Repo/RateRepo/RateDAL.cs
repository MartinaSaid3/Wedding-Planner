
using Data_Access_Layer.Context;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repo.RateRepo;

public class RateDAL : IRateDAL
{
    private readonly ApplicationEntity _context;

    public RateDAL(ApplicationEntity context)
    {
        _context = context;
    }

    public Task<double> GetAverageRateForVenueAsync(int venueId)
    {
        return _context.Rates
             .Where(r => r.Reservation.VenueId == venueId)
             .AverageAsync(r => r.Rating);
    }
}

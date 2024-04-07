using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repo.RateRepo;

public interface IRateDAL
{
    Task AddRating(Rate rate);
    Task<double> GetAverageRateForVenueOrDefaultAsync(int venueId);

    Task<Rate> GetRatingByVenueAndUser(int venueId, int userId);
   
    
    Task<List<Rate>> GetRatingsForVenue(int venueId);
}

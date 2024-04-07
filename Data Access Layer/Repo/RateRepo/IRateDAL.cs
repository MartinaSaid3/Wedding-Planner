namespace Data_Access_Layer.Repo.RateRepo;

public interface IRateDAL
{
    Task<double> GetAverageRateForVenueAsync(int venueId);
}

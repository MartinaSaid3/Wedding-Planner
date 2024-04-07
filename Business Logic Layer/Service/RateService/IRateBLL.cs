using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.RateService
{
    public interface IRateBLL
    {
        Task AddRating(Rate rating);

        Task<Rate> GetRatingByVenueAndUser(int venueId, int userId);
    }
}

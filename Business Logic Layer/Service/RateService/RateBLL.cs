using Data_Access_Layer.Models;
using Data_Access_Layer.Repo.RateRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.RateService
{
    public class RateBLL :IRateBLL
    {
        private readonly IRateDAL rateDAL;
        public RateBLL(IRateDAL _rateDAL)
        {
            rateDAL = _rateDAL;
        }

        public async Task AddRating(Rate rating)
        {
            await rateDAL.AddRating(rating);
        }





        public async Task<Rate> GetRatingByVenueAndUser(int venueId, int userId)
        {

            return await rateDAL.GetRatingByVenueAndUser(venueId, userId);
        }
    }
}

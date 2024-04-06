using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Repo.AccountRepo
{
     public interface IAccountDAL
     {
        Task<ServicesResult<ApplicationUser>> FindUserByNameAsync(string username);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserToResetPassword(string userName);


        //Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        //Task<IEnumerable<ApplicationUser>> GetAllUsersByAdmin();

        //Task<ApplicationUser> GetByIdAsync(string UserName);
    }
}

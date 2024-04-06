using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Repo.AdminRepo
{
    public interface IAdminDAL
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

        Task<IEnumerable<ApplicationUser>> GetAllUsersByAdmin();

        Task<ApplicationUser> GetByIdAsync(string UserName);

        Task<bool> DeleteUser(string UserId);

        Task<ApplicationUser> Update(string UserName);
    }
}

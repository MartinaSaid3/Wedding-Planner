using Business_Logic_Layer.Dtos.AccountDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.AdminServices
{
    public interface IAdminBLL
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        Task<IEnumerable<GetAllUserDataDto>> GetAllUsersByadmin();

        Task<IEnumerable<GetAllUserDataDto>> GetById(string UserName);

        Task<bool> DeleteUser(string UserId);
        Task<bool> UpdateUser(string userName, GetAllUserDataDto userData);
    }
}

using Business_Logic_Layer.Dtos.AccountDtos;
using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.AccountServices
{
    public interface IAccountBLL
    {
        Task<ServicesResult<ApplicationUser>> Registration(RegisterUserDto UserDto);

        Task<ServicesResult<TokenDto>> LoginAsync(LoginUserDto userDto);
        Task<ServicesResult<ApplicationUser>> ForgetPasswordAsync(ForgetPasswordDto model);

        Task<ServicesResult<ApplicationUser>> ResetPassword(ResetPasswordDto model);

        //Task<IEnumerable<UserDto>> GetAllUsersAsync();

        //Task<IEnumerable<GetAllUserDataDto>> GetAllUsersByadmin();

        //Task<IEnumerable<GetAllUserDataDto>> GetById(string UserName);
    }
}

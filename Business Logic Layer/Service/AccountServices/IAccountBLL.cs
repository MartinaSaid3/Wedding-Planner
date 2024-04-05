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
        Task<ServicesResult<string>> LoginAsync(LoginUserDto userDto);

        Task<ServicesResult<ApplicationUser>> ForgetPasswordAsync(ForgetPasswordDto model);

        Task<ServicesResult<ApplicationUser>> ResetPassword(ResetPasswordDto model);
    }
}

using Business_Logic_Layer.Dtos.AccountDtos;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repo.AdminRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.AdminServices
{
    public class AdminBLL:IAdminBLL
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration Config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Data_Access_Layer.Repo.AdminRepo.IAdminDAL adminDAL;
        private readonly LinkGenerator _linkGenerator;
        private Business_Logic_Layer.Service.EmailService.IEmailSender _emailSender;
        public AdminBLL(UserManager<ApplicationUser> UserManger,
            IConfiguration Config,
            IHttpContextAccessor httpContextAccessor,
             IAdminDAL _adminDAL,
            LinkGenerator linkGenerator,
            EmailService.IEmailSender emailSender)
        {
            this.userManager = UserManger;
            this.Config = Config;
            _httpContextAccessor = httpContextAccessor;
            adminDAL = _adminDAL;
            _linkGenerator = linkGenerator;
            _emailSender = emailSender;
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await adminDAL.GetAllUsersAsync();

            // Convert ApplicationUser objects to UserDto objects
            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                userDtos.Add(new UserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Role = user.Role // Assuming you have a property representing the user's role in ApplicationUser
                });
            }

            return userDtos;
        }

        public async Task<IEnumerable<GetAllUserDataDto>> GetAllUsersByadmin()
        {
            var Users = await adminDAL.GetAllUsersByAdmin();

            // Convert ApplicationUser objects to UserDto objects
            var userDto = new List<GetAllUserDataDto>();
            foreach (var User in Users)
            {
                userDto.Add(new GetAllUserDataDto
                {
                    UserId = User.Id,
                    UserName = User.UserName,
                    Email = User.Email,
                    Role = User.Role,// Assuming you have a property representing the user's role in ApplicationUser
                    Address = User.UserLocation,
                    Gender = User.Gender,
                    Phone = User.PhoneNumber,
                    SSN = User.SSN,
                });
            }

            return userDto;
        }

        public async Task<IEnumerable<GetAllUserDataDto>> GetById(string UserName)
        {
            var user = await adminDAL.GetByIdAsync(UserName);
            if (user != null)
            {
                var userDto = new GetAllUserDataDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role,
                    Address = user.UserLocation,
                    Gender = user.Gender,
                    Phone = user.PhoneNumber,
                    SSN = user.SSN
                };
                return new List<GetAllUserDataDto> { userDto };
            }
            else
            {
                // Handle case where user with the given ID is not found
                return Enumerable.Empty<GetAllUserDataDto>(); // Return an empty collection
            }
        }

        public async Task<bool> DeleteUser(string UserId)
        {
            // You can implement additional validation or authorization logic here

            return await adminDAL.DeleteUser(UserId);
        }

        public async Task<bool> UpdateUser(string userName, GetAllUserDataDto userData)
        {
            var userToUpdate = await adminDAL.Update(userName);

            if (userToUpdate == null)
            {
                return false; // User not found
            }


            userToUpdate.Email = userData.Email;
            userToUpdate.Role = userData.Role;
            userToUpdate.PhoneNumber = userData.Phone;
            userToUpdate.Gender = userData.Gender;

            userToUpdate.SSN = userData.SSN;
            // Update other properties as needed

            // Save changes to the database


            return true; // User successfully updated
        }



    }
}


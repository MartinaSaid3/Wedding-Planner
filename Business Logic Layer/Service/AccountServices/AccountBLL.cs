using Azure.Core;
using Business_Logic_Layer.Dtos.AccountDtos;
using CloudinaryDotNet;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repo.AccountRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.AccountServices
{
    public class AccountBLL :IAccountBLL
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration Config;
        private Data_Access_Layer.Repo.AccountRepo.IAccountDAL accountDAL;
        public AccountBLL(UserManager<ApplicationUser> UserManger, IConfiguration Config , IAccountDAL _accountDAL)
        {
            this.userManager = UserManger;
            this.Config = Config;
            accountDAL = _accountDAL;
        }

        public async Task<ServicesResult<ApplicationUser>> Registration(RegisterUserDto UserDto)
        {
           
                //save 
                ApplicationUser user = new ApplicationUser();
                user.UserName = UserDto.UserName;
                user.Email = UserDto.Email;
                user.PhoneNumber = UserDto.Phone;
                user.Role = UserDto.Role;
                user.Gender = UserDto.Gender;
                user.UserLocation = UserDto.Address;
                user.SSN = UserDto.SSN;

                //IdentityResult result = await userManager.CreateAsync(user, UserDto.Password);

             var result = await userManager.CreateAsync(user, UserDto.Password);
             if (result.Succeeded)
            {
                return ServicesResult<ApplicationUser>.Successed(user ,"success");
            }

            return ServicesResult<ApplicationUser>.Failure(result.Errors.FirstOrDefault()?.Description ?? "Failed to create user.");

        }

        public async Task<ServicesResult<string>> LoginAsync(LoginUserDto userDto)
        {
            var result = await accountDAL.FindUserByNameAsync(userDto.UserName);
            if (result.Success)
            {
                var user = result.Data;
                var found = await accountDAL.CheckPasswordAsync(user, userDto.Password);
                if (found)
                {
                    var token = GenerateJwtToken(user);
                    return ServicesResult<string>.Successed(token);
                }
            }
            return ServicesResult<string>.Failure("Unauthorized");
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var roles = accountDAL.GetRolesAsync(user).Result;
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["JWT:Secret"]));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: Config["JWT:ValidIssuer"],
                audience: Config["JWT:ValidAudiance"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }

        public async Task<ServicesResult<ApplicationUser>> ForgetPassword(ForgetPasswordDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return ServicesResult<ApplicationUser>.Failure("If your email is registered, you will receive instructions to reset your password.");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            //var emailService = new EmailService("smtp.gmail.com", 587, "your_email@gmail.com", "your_gmail_password");
            //await emailService.SendEmailAsync(user.Email, "Password Reset", "Please click the following link to reset your password: " + resetLink);

            return ServicesResult<ApplicationUser>.Successed(default,"If your email is registered, you will receive instructions to reset your password");
        }

        public async Task<ServicesResult<ApplicationUser>> ResetPassword(ResetPasswordDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
               
                throw new InvalidOperationException("Invalid Email.");
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return ServicesResult<ApplicationUser>.Successed(default,"Password reset successful.");
            }

            throw new InvalidOperationException("Invalid token or password reset failed.");
        }
    }
}


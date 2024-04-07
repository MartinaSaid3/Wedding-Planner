using Azure.Core;
using Business_Logic_Layer.Dtos.AccountDtos;
using Business_Logic_Layer.Dtos.ReservationDtos;
using Business_Logic_Layer.Service.EmailService;
using Business_Logic_Layer.Service.ReservationService;
using CloudinaryDotNet;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repo.AccountRepo;
using Data_Access_Layer.Repo.ReservationRepo;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Business_Logic_Layer.Service.AccountServices;
//public record OmniaLoginDto(string Token, string Role);

public class AccountBLL : IAccountBLL
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IConfiguration Config;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Data_Access_Layer.Repo.AccountRepo.IAccountDAL accountDAL;
    private readonly LinkGenerator _linkGenerator;
    private Business_Logic_Layer.Service.EmailService.IEmailSender _emailSender;
    public AccountBLL(UserManager<ApplicationUser> UserManger,
        IConfiguration Config,
        IHttpContextAccessor httpContextAccessor,
        IAccountDAL _accountDAL,
        LinkGenerator linkGenerator,
        EmailService.IEmailSender emailSender)
    {
        this.userManager = UserManger;
        this.Config = Config;
        _httpContextAccessor = httpContextAccessor;
        accountDAL = _accountDAL;
        _linkGenerator = linkGenerator;
        _emailSender = emailSender;
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
            return ServicesResult<ApplicationUser>.Successed(user, "success");
        }

        return ServicesResult<ApplicationUser>.Failure(result.Errors.FirstOrDefault()?.Description ?? "Failed to create user.");

    }

    public async Task<ServicesResult<TokenDto>> LoginAsync(LoginUserDto userDto)
    {
        var result = await accountDAL.FindUserByNameAsync(userDto.UserName);
        if (result.Success)
        {
            var user = result.Data;
            var found = await accountDAL.CheckPasswordAsync(user, userDto.Password);
            if (found)
            {
                var token = GenerateJwtToken(user);
                return ServicesResult<TokenDto>.Successed(new TokenDto(token, result.Data.Role));
            }
        }
        return ServicesResult<TokenDto>.Failure("Unauthorized");
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
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

    public async Task<ServicesResult<ApplicationUser>> ForgetPasswordAsync(ForgetPasswordDto model)
    {
        var user = await userManager.FindByNameAsync(model.UserName);
        if (user == null)
        {
            // Don't reveal that the user does not exist or is not confirmed
            return ServicesResult<ApplicationUser>.Failure("If your email is registered, you will receive instructions to reset your password.");
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        //var resetLink = _urlHelper.Action("ResetPassword",
        //    "Account",
        //    new { email = model.Email, token = token },
        //    _httpContextAccessor.HttpContext?.Request.Scheme);

        var resetLink = _linkGenerator.GetUriByAction("ResetPassword",
            "Account",
            new { email = model.Email, token = token },
            _httpContextAccessor.HttpContext!.Request.Scheme,
            _httpContextAccessor.HttpContext!.Request.Host);
        var urlEncodedToken = HttpUtility.UrlEncode(token);
        string url = $"http://localhost:4200/account/reset?token={urlEncodedToken}";

        var forReset = await userManager.FindByNameAsync(user.UserName!);
        if (forReset != null)
        {
            //BackgroundJob.Enqueue(() => _emailSender.SendEmail("Reset Password", forReset.Email!, forReset.Email!, $"<a href = {resetLink}>Click Here</a>", "To reset your password, please click on this link: "));
            BackgroundJob.Enqueue(() => _emailSender.SendEmail("Reset Password", forReset.Email!, forReset.Email!, $"<a href = {url}>Click Here</a>", "To reset your password, please click on this link: "));

        }
        else
            return ServicesResult<ApplicationUser>.Failure("Invalid Email.");


        //var emailService = new EmailService("smtp.gmail.com", 587, "your_email@gmail.com", "your_gmail_password");
        //await emailService.SendEmailAsync(user.Email, "Password Reset", "Please click the following link to reset your password: " + resetLink);

        return ServicesResult<ApplicationUser>.Successed(default!, "If your email is registered, you will receive instructions to reset your password");
    }



    public async Task<ServicesResult<ApplicationUser>> ResetPassword(ResetPasswordDto model)
    {
        var user = await userManager.FindByNameAsync(model.UserName);
        if (user == null)
        {
            // Don't reveal that the user does not exist

            throw new InvalidOperationException("Invalid Account.");
        }

        var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (result.Succeeded)
        {
            return ServicesResult<ApplicationUser>.Successed(default, "Password reset successful.");
        }

        throw new InvalidOperationException("Invalid token or password reset failed.");
    }

    //public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    //{
    //    var users = await accountDAL.GetAllUsersAsync();

    //    // Convert ApplicationUser objects to UserDto objects
    //    var userDtos = new List<UserDto>();
    //    foreach (var user in users)
    //    {
    //        userDtos.Add(new UserDto
    //        {
    //            Username = user.UserName,
    //            Email = user.Email,
    //            Role = user.Role // Assuming you have a property representing the user's role in ApplicationUser
    //        });
    //    }

    //    return userDtos;
    //}

    //public async Task<IEnumerable<GetAllUserDataDto>> GetAllUsersByadmin()
    //{
    //    var Users = await accountDAL.GetAllUsersByAdmin();

    //    // Convert ApplicationUser objects to UserDto objects
    //    var userDto = new List<GetAllUserDataDto>();
    //    foreach (var User in Users)
    //    {
    //        userDto.Add(new GetAllUserDataDto
    //        {

    //            UserName = User.UserName,
    //            Email = User.Email,
    //            Role = User.Role,// Assuming you have a property representing the user's role in ApplicationUser
    //            Address = User.UserLocation,
    //            Gender = User.Gender,
    //            Phone = User.PhoneNumber,
    //            SSN = User.SSN,
    //        });
    //    }

    //    return userDto;
    //}

    //public async Task<IEnumerable<GetAllUserDataDto>> GetById(string UserName)
    //{
    //    var user = await accountDAL.GetByIdAsync(UserName);
    //    if (user != null)
    //    {
    //        var userDto = new GetAllUserDataDto
    //        {
    //            UserName = user.UserName,
    //            Email = user.Email,
    //            Role = user.Role,
    //            Address = user.UserLocation,
    //            Gender = user.Gender,
    //            Phone = user.PhoneNumber,
    //            SSN = user.SSN
    //        };
    //        return new List<GetAllUserDataDto> { userDto };
    //    }
    //    else
    //    {
    //        // Handle case where user with the given ID is not found
    //        return Enumerable.Empty<GetAllUserDataDto>(); // Return an empty collection
    //    }
    //}
}


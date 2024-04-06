using Business_Logic_Layer.Dtos.AccountDtos;
using Business_Logic_Layer.Service.AccountServices;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Wedding_Planner_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private Business_Logic_Layer.Service.AccountServices.IAccountBLL _accountBLL;


        public AccountController(Business_Logic_Layer.Service.AccountServices.IAccountBLL accountBLL)
        {
            _accountBLL = accountBLL;

        }

        //create account "post" new user  "registration"
        [HttpPost("register")] //api/account/register
        public async Task<IActionResult> Registration(RegisterUserDto UserDto)
        {
            if (ModelState.IsValid)
            {

                var result = await _accountBLL.Registration(UserDto);
                if (result.Success)
                {
                    return Ok(new { message = "success", user = result.Data });
                }
            }
            return BadRequest(ModelState);
        }

        // check Account Valid "login" "post"
        [HttpPost("Login")] //api/account /login
        public async Task<IActionResult> LoginAsync(LoginUserDto userDto)
        {
            var result = await _accountBLL.LoginAsync(userDto);

            if (result.Success)
            {

                return Ok(result);
            }
            return Unauthorized(result.Message);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto model)
        {
            var result = await _accountBLL.ForgetPasswordAsync(model);
            if (result.Success)
            {
                return NoContent();
            }
            return BadRequest();
            //var user = await userManager.FindByEmailAsync(model.Email);
            //if (user == null)
            //{
            //    // Don't reveal that the user does not exist or is not confirmed
            //    return Ok("If your email is registered, you will receive instructions to reset your password.");
            //}

            //var token = await userManager.GeneratePasswordResetTokenAsync(user);
            //var resetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

            //return Ok("If your email is registered, you will receive instructions to reset your password.");
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            await _accountBLL.ResetPassword(model);
            return Ok();
        }

        //[HttpPost("Logout")]
        //public async Task<IActionResult> LogOutAsync()
        //{
        //    await HttpContext.SignOutAsync();
        //    return Ok("Logged Out Successfully");
        //}

        //get role 
        //[HttpGet("UserRole")]

        //// [Authorize(Roles = "Admin")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await _accountBLL.GetAllUsersAsync();
        //    return Ok(users);
        //}

        ////get ahh data of user 
        //[HttpGet("GetAllUser")]
        //public async Task<IActionResult> GetAllUserbyAdmin()
        //{
        //    var users = await _accountBLL.GetAllUsersByadmin();
        //    return Ok(users);
        //}

        ////get user by id

        ////[HttpGet("{UserName}")]
        ////public async Task<IActionResult> GetUserById(string UserName)
        ////{
        ////    var user = await _accountBLL.GetById(UserName);
        ////    if (user == null)
        ////    {
        ////        return NotFound();
        ////    }
        ////    return Ok(user);
        ////}
        ////get by name
        //[HttpGet("GetByUsername")]
        //public async Task<IActionResult> GetUserByUsername(string username)
        //{
        //    var user = await _accountBLL.GetById(username);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}

    }

}




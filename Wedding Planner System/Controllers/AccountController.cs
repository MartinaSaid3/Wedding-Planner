using Business_Logic_Layer.Dtos.AccountDtos;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Wedding_Planner_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        //injection //need to inject services
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration Config;

        public AccountController(UserManager<ApplicationUser> UserManger, IConfiguration Config)
        {
            this.userManager = UserManger;
            this.Config = Config;
        }

        //create account "post" new user  "registration"
        [HttpPost("register")] //api/account/register
        public async Task<IActionResult> Registration(RegisterUserDto UserDto)
        {
            if (ModelState.IsValid)
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

                IdentityResult result = await userManager.CreateAsync(user, UserDto.Password);
                if (result.Succeeded)
                {
                    return Ok(new { message = "success" });
                }
                return BadRequest(result.Errors.FirstOrDefault());
            }
            return BadRequest(ModelState);
        }

        // check Account Valid "login" "post"
        [HttpPost("Login")] //api/account /login
        public async Task<IActionResult> LoginAsync(LoginUserDto UserDto)
        {
            if (ModelState.IsValid == true)
            {
                //check and create token
                ApplicationUser user = await userManager.FindByNameAsync(UserDto.UserName);
                if (user != null) //user name found 
                {
                    bool Found = await userManager.CheckPasswordAsync(user, UserDto.Password);
                    if (Found)
                    {
                        //claims token ,, things added to token
                        var Claims = new List<Claim>();
                        Claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        Claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                        //id unique to token
                        Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));


                        //get role 
                        var Roles = await userManager.GetRolesAsync(user);
                        foreach (var RoleItem in Roles)
                        {
                            //added role to token   ,,represent token
                            Claims.Add(new Claim(ClaimTypes.Role, RoleItem));
                        }
                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["JWT:Secret"]));

                        SigningCredentials Signincred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        //create token by jwt secrete token (buildin create token)
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                          issuer: Config["JWT:ValidIssuer"], //url provider
                          audience: Config["JWT:ValidAudiance"], //url consumer angular
                          claims: Claims,    //name and role to token
                          expires: DateTime.Now.AddHours(1), //expire date 
                          signingCredentials: Signincred
                            );
                        //return token
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),      //create token
                            expiration = mytoken.ValidTo
                        });

                    }
                }
                return Unauthorized();
            }
            return Unauthorized();
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Ok("If your email is registered, you will receive instructions to reset your password.");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
            //var emailService = new EmailService("smtp.gmail.com", 587, "your_email@gmail.com", "your_gmail_password");
            //await emailService.SendEmailAsync(user.Email, "Password Reset", "Please click the following link to reset your password: " + resetLink);



            return Ok("If your email is registered, you will receive instructions to reset your password.");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return BadRequest("Invalid email.");
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {


                return Ok("Password reset successful.");
            }

            return BadRequest("Invalid token or password reset failed.");
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync();
            return Ok("Logged Out Successfully");
        }

    }


}

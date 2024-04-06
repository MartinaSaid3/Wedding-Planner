using Business_Logic_Layer.Dtos.AccountDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wedding_Planner_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private Business_Logic_Layer.Service.AdminServices.IAdminBLL _adminBLL;


        public AdminController(Business_Logic_Layer.Service.AdminServices.IAdminBLL adminBLL)
        {
            _adminBLL = adminBLL;

        }
        //get role 
        [HttpGet("UserRole")]

        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminBLL.GetAllUsersAsync();
            return Ok(users);
        }

        //get ahh data of user 
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUserbyAdmin()
        {
            var users = await _adminBLL.GetAllUsersByadmin();
            return Ok(users);
        }

        //get user by id

        //[HttpGet("{UserName}")]
        //public async Task<IActionResult> GetUserById(string UserName)
        //{
        //    var user = await _accountBLL.GetById(UserName);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}
        //get by name
        [HttpGet("GetByUsername")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _adminBLL.GetById(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpDelete("{UserId}")]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            var success = await _adminBLL.DeleteUser(UserId);

            if (success)
            {
                return Ok("User deleted successfully");
            }
            else
            {
                return NotFound("User not found or unable to delete user");
            }
        }





        [HttpPut("{userName}")]
        public async Task<IActionResult> UpdateUser(string userName, [FromBody] GetAllUserDataDto userData)
        {
            var success = await _adminBLL.UpdateUser(userName, userData);

            if (success)
            {
                return Ok("User updated successfully");
            }
            else
            {
                return NotFound("User not found or unable to update user");
            }
        }

    }
}


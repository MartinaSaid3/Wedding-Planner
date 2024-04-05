using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Repo.AccountRepo
{
    public class AccountDAL:IAccountDAL
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountDAL(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ServicesResult<ApplicationUser>> FindUserByNameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                return ServicesResult<ApplicationUser>.Successed(user);
            }
            return ServicesResult<ApplicationUser>.Failure("User not found");
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }

}


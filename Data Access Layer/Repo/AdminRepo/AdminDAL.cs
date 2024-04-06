using Data_Access_Layer.Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;

namespace Data_Access_Layer.Repo.AdminRepo
{
    public class AdminDAL:IAdminDAL
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationEntity _context;

        public AdminDAL(UserManager<ApplicationUser> userManager, ApplicationEntity context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersByAdmin()
        {
            return await _context.Users.ToListAsync();
        }

        //public async Task<ApplicationUser> GetByIdAsync(string UserName)
        //{
        //    return await _context.Users.FindAsync(UserName);
        //}

        public async Task<ApplicationUser> GetByIdAsync(string UserName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == UserName);
        }

        public async Task<bool> DeleteUser(string UserId)
        {
            var userToDelete = await _userManager.FindByIdAsync(UserId);

            if (userToDelete == null)
                return false; // User not found

            var result = await _userManager.DeleteAsync(userToDelete);

            return result.Succeeded; // Return true if user is successfully deleted, false otherwise
        }

        public async Task<ApplicationUser> Update(string UserName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == UserName);
            await _context.SaveChangesAsync();
        }


    }
}


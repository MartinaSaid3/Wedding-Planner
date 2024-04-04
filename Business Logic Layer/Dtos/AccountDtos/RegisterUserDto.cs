using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Dtos.AccountDtos
{
    public class RegisterUserDto
    {
        //make to not use application user to make custome proprty and add validation

        public string FullName { get; set; }

        public string Role { get; set; }

        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }

        public string Address { get; set; }

        public string SSN { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "password is required ")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }

    }
}

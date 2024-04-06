using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Dtos.AccountDtos
{
    public class GetAllUserDataDto
    {
        public string UserId { get; set; }
        // public string FullName { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string SSN { get; set; }

        public string UserName { get; set; }

        //public string Password { get; set; }
        //public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}

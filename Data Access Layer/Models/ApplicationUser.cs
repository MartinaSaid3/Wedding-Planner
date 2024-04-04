using Microsoft.AspNetCore.Identity; //provides functionality for managing users, roles, and authentication in ASP.NET Core applications
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class ApplicationUser : IdentityUser //This includes features like password hashing, email confirmation, two-factor authentication, and more
    {

            public string Role { get; set; }
            public string Gender { get; set; }
            public string UserLocation { get; set; }
            public string SSN { get; set; }
    }
}

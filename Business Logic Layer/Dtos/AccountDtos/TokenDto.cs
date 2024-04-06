using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Dtos.AccountDtos
{
    public class TokenDto
    {
        public string Token { get; set; }
        public string Role { get; set; }

        public TokenDto(string Token , string Role)
        {
            this.Token = Token;
            this.Role = Role;
        }
    }
}

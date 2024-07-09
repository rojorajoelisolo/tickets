using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tickets.Domain.Requests.Auth
{
    public class AuthLoginRequest
    {
        public required string UserName { get; set; }

        public required string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Responses.Base;

namespace tickets.Domain.Responses.Auth
{
    public class AuthLoginResponse : AbstractResponse
    {
        public string? Token { get; set; }
    }
}

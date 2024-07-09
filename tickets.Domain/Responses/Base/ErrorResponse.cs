using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tickets.Domain.Responses.Base
{
    public class ErrorResponse : AbstractResponse
    {
        public string? Message { get; set; }
    }
}

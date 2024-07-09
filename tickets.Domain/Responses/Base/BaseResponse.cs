using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tickets.Domain.Responses.Base
{
    public class BaseResponse<TBody> where TBody : AbstractResponse
    {
        public bool Status { get; set; } = true;

        public TBody? Data { get; set; }
    }
}

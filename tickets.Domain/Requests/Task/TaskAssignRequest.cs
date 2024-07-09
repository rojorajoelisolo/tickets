using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tickets.Domain.Requests.Task
{
    public class TaskAssignRequest
    {
        public required List<string>? Emails { get; set; }
    }
}

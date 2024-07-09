using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tickets.Domain.Requests.Task
{
    public class TaskProcessRequest
    {
        public required int TaskId { get; set; }
    }
}

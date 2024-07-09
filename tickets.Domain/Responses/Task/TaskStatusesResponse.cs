using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Responses.Base;
using StatusEnum = tickets.Domain.Enums.Status;

namespace tickets.Domain.Responses.Task
{
    public class TaskStatusesResponse : AbstractResponse
    {
        public IList<string>? Statuses { get; set; }
    }
}

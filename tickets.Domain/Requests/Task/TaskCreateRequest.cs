using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Enums;

namespace tickets.Domain.Requests.Task
{
    public class TaskCreateRequest
    {
        public required string Title { get; set; }

        public string? Content { get; set; }

        public required int Priority { get; set; }
    }
}

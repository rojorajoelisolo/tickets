using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Enums;

namespace tickets.Domain.Entities
{
    public class TaskStatus
    {
        public int Id { get; set; }

        //public int TaskId { get; set; }
        public required Task Task { get; set; }

        public Status Status { get; set; }

        //public int UpdatedById { get; set; }
        public required User UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}

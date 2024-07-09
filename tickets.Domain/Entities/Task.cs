using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Enums;

namespace tickets.Domain.Entities
{
    public class Task
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public string? Content { get; set; }

        public int Priority { get; set; }

        public Status CurrentStatus { get; set; } = Status.ToDo;

        //public int CreatedById { get; set; }
        public required User CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public IList<User>? Users { get; set; } = new List<User>();

    }
}

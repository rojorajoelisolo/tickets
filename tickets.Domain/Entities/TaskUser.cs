using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tickets.Domain.Entities
{
    public class TaskUser
    {
        public int Id { get; set; }

        //public int TaskId { get; set; }
        public required Task Task { get; set; }

        //public string UsersId { get; set; }
        public required User User { get; set; }

        //public string AddedById { get; set; }
        public required User AddedBy { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}

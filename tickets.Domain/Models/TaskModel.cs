using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Entities;

namespace tickets.Domain.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public int Priority { get; set; }

        public string? CurrentStatus { get; set; }

        public string? CreatedBy { get; set; }

        public string? CreatedOn { get; set; }

        public IList<UserModel>? Users { get; set; } = new List<UserModel>();
    }
}

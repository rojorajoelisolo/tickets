﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tickets.Domain.Models
{
    public class UserModel
    {
        public string? Id { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public IList<TaskModel>? Tasks { get; set; } = new List<TaskModel>();
    }
}
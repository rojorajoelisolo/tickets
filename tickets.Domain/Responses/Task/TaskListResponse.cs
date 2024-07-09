﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Models;
using tickets.Domain.Responses.Base;

namespace tickets.Domain.Responses.Task
{
    public class TaskListResponse : AbstractResponse
    {
        public IList<TaskModel>? Tasks { get; set; }
    }
}
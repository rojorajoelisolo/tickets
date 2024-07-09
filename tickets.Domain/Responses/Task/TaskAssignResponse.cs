﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Models;
using tickets.Domain.Responses.Base;

namespace tickets.Domain.Responses.Task
{
    public class TaskAssignResponse : AbstractResponse
    {
        public TaskModel? Task { get; set; }

        public IList<string>? SuccessAssigned { get; set; }

        public IList<string>? AlreadyAssigned { get; set; }

        public IList<string>? ErrorAssigned { get; set; }
    }
}
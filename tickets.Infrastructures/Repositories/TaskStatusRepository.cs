using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Contexts;
using tickets.Interfaces.Repositories;
using TaskStatusEntity = tickets.Domain.Entities.TaskStatus;

namespace tickets.Infrastructures.Repositories
{
    public class TaskStatusRepository : BaseRepository<TaskStatusEntity>, ITaskStatusRepository
    {
        public TaskStatusRepository(TicketsContext ticketsContext) : base(ticketsContext)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatusEntity = tickets.Domain.Entities.TaskStatus;

namespace tickets.Interfaces.Repositories
{
    public interface ITaskStatusRepository : IBaseRepository<TaskStatusEntity>
    {
    }
}

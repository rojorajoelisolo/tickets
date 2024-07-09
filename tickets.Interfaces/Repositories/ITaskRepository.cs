using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskEntity = tickets.Domain.Entities.Task;

namespace tickets.Interfaces.Repositories
{
    public interface ITaskRepository : IBaseRepository<TaskEntity>
    {
        Task<IList<TaskEntity>> GetAllTasksAsync();

        Task<TaskEntity> GetTaskByIdAsync(int id);

        Task<IList<TaskEntity>> GetTasksByUserId(string userId);

    }
}

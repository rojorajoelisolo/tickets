using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskUserEntity = tickets.Domain.Entities.TaskUser;

namespace tickets.Interfaces.Repositories
{
    public interface ITaskUserRepository : IBaseRepository<TaskUserEntity>
    {
        Task<bool> IsTaskUserExistsAsync(int taskId, string userId);
    }
}

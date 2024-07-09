using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tickets.Domain.Contexts;
using tickets.Interfaces.Repositories;
using TaskUserEntity = tickets.Domain.Entities.TaskUser;

namespace tickets.Infrastructures.Repositories
{
    public class TaskUserRepository : BaseRepository<TaskUserEntity>, ITaskUserRepository
    {
        public TaskUserRepository(TicketsContext ticketsContext) : base(ticketsContext)
        {
        }

        public async Task<bool> IsTaskUserExistsAsync(int taskId, string userId)
        {
            return await _ticketsContext.TaskUsers.AnyAsync(tu => tu.Task.Id == taskId && tu.User.Id == userId);
        }
    }
}

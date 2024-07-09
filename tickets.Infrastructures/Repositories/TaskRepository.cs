using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Contexts;
using tickets.Domain.Entities;
using tickets.Interfaces.Repositories;
using TaskEntity = tickets.Domain.Entities.Task;
using UserEntity = tickets.Domain.Entities.User;

namespace tickets.Infrastructures.Repositories
{
    public class TaskRepository : BaseRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(TicketsContext ticketsContext) : base(ticketsContext)
        {
        }

        public async Task<IList<TaskEntity>> GetAllTasksAsync()
        {
            var tasks = await _ticketsContext.Tasks
                .Include(t => t.CreatedBy)
                .ToListAsync();
            return tasks;
        }

        public async Task<TaskEntity> GetTaskByIdAsync(int id)
        {
            var task = await _ticketsContext.Tasks
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(t => t.Id == id);
            return task;
        }

        public async Task<IList<TaskEntity>> GetTasksByUserId(string userId)
        {
            var tasks = await (from task in _ticketsContext.Tasks
                               where _ticketsContext.TaskUsers.Any(tu => tu.Task.Id == task.Id && tu.User.Id == userId)
                               select new TaskEntity
                               {
                                   Id = task.Id,
                                   Title = task.Title,
                                   Content = task.Content,
                                   Priority = task.Priority,
                                   CreatedBy = task.CreatedBy,
                                   Users = (from tu in _ticketsContext.TaskUsers
                                            where tu.Task.Id == task.Id
                                            join u in _ticketsContext.Users on tu.User.Id equals u.Id
                                            select new UserEntity
                                            {
                                                Id = u.Id,
                                                UserName = u.UserName,
                                                Email = u.Email
                                            }).ToList()
                               }).ToListAsync();
            return tasks;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Enums;
using tickets.Domain.Requests.Task;
using tickets.Domain.Responses.Base;
using tickets.Domain.Responses.Task;

namespace tickets.Interfaces.Services
{
    public interface ITaskService
    {
        Task<BaseResponse<TaskCreateResponse>> Create(TaskCreateRequest request, ClaimsPrincipal currentUser);

        Task<BaseResponse<TaskAssignResponse>> Assign(int id, TaskAssignRequest request, ClaimsPrincipal currentUser);

        Task<BaseResponse<TaskDeleteResponse>> Delete(int id, ClaimsPrincipal currentUser);

        Task<BaseResponse<TaskProcessResponse>> Process(int id, ClaimsPrincipal currentUser);

        Task<BaseResponse<TaskListResponse>> GetAll();

        Task<BaseResponse<TaskResponse>> GetById(int id);

        Task<BaseResponse<TaskListResponse>> GetAllMyTasks(ClaimsPrincipal currentUser);

        Task<BaseResponse<TaskListResponse>> GetAllTasksOf(TaskOfRequest request);

        Task<BaseResponse<TaskMarkAsResponse>> MarkAs(TaskMarkAsRequest request, ClaimsPrincipal currentUser);

        BaseResponse<TaskStatusesResponse> GetAllStatus();
    }
}

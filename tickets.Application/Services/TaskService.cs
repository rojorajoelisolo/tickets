using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Requests.Task;
using tickets.Domain.Responses.Base;
using tickets.Domain.Responses.Task;
using tickets.Interfaces.Repositories;
using tickets.Interfaces.Services;
using TaskEntity = tickets.Domain.Entities.Task;
using UserEntity = tickets.Domain.Entities.User;
using TaskStatusEntity = tickets.Domain.Entities.TaskStatus;
using TaskUserEntity = tickets.Domain.Entities.TaskUser;
using StatusEnum = tickets.Domain.Enums.Status;
using Microsoft.AspNetCore.Identity;
using tickets.Domain.Models;
using tickets.Domain.Responses.Auth;

namespace tickets.Application.Services
{
    public class TaskService : BaseService, ITaskService
    {
        public ITaskRepository _taskRepository;

        public ITaskStatusRepository _taskStatusRepository;

        public ITaskUserRepository _taskUserRepository;

        public UserManager<UserEntity> _userManager;

        public IAuthService _authService;

        public TaskService(
            ITaskRepository taskRepository,
            ITaskStatusRepository taskStatusRepository,
            ITaskUserRepository taskUserRepository,
            UserManager<UserEntity> userManager,
            IAuthService authService,
            IMapper mapper
        ) : base(mapper)
        {
            _taskRepository = taskRepository;
            _taskStatusRepository = taskStatusRepository;
            _taskUserRepository = taskUserRepository;
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<BaseResponse<TaskAssignResponse>> Assign(int id, TaskAssignRequest request, ClaimsPrincipal currentUser)
        {
            if (!currentUser.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("The user is not authenticated.");
            }
            var (assigneds, alreadyassigneds, unassigneds) = (new List<string>(), new List<string>(), new List<string>());
            var taskUsers = new List<TaskUserEntity>();

            var task = await _taskRepository.GetByIdAsync(id);
            var userEmail = currentUser.FindFirstValue(ClaimTypes.Email);
            var userEntity = await _userManager.FindByEmailAsync(userEmail);

            foreach (var email in request.Emails)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var alreadyAssigned = await _taskUserRepository.IsTaskUserExistsAsync(task.Id, user.Id);
                    if (!alreadyAssigned)
                    {
                        assigneds.Add(user.Email);
                        taskUsers.Add(new TaskUserEntity
                        {
                            Task = task,
                            AddedBy = userEntity,
                            User = user
                        });
                    }
                    else
                    {
                        alreadyassigneds.Add(user.Email);
                    }
                }
                else
                {
                    unassigneds.Add(email);
                }
            }
            
            if (taskUsers.Count > 0)
            {
                var assigned = await _taskUserRepository.AddRangeAsync(taskUsers);
                var updated = await MarkStatusAs(StatusEnum.Started, task, userEntity);
            }

            var taskModel = _mapper.Map<TaskModel>(task);
            return new BaseResponse<TaskAssignResponse>
            {
                Data = new TaskAssignResponse
                {
                    Task = taskModel,
                    SuccessAssigned = assigneds,
                    AlreadyAssigned = alreadyassigneds,
                    ErrorAssigned = unassigneds
                }
            };
        }

        public async Task<BaseResponse<TaskCreateResponse>> Create(TaskCreateRequest request, ClaimsPrincipal currentUser)
        {
            if (currentUser.Identity.IsAuthenticated)
            {
                var userEmail = currentUser.FindFirstValue(ClaimTypes.Email);
                var userEntity = await _userManager.FindByEmailAsync(userEmail);

                var taskEntity = _mapper.Map<TaskEntity>(request);
                taskEntity.CreatedBy = userEntity;

                var taskCreated = await _taskRepository.AddAsync(taskEntity);

                var taskStatus = new TaskStatusEntity
                {
                    Task = taskCreated,
                    Status = StatusEnum.ToDo,
                    UpdatedBy = userEntity
                };
                await _taskStatusRepository.UpdateAsync(taskStatus);
                
                var taskCreatedModel = _mapper.Map<TaskModel>(taskCreated);

                return new BaseResponse<TaskCreateResponse>
                {
                    Data = new TaskCreateResponse
                    {
                        Task = taskCreatedModel
                    }
                };
            }
            throw new UnauthorizedAccessException("The user is not authenticated.");
        }

        public async Task<BaseResponse<TaskDeleteResponse>> Delete(int id, ClaimsPrincipal currentUser)
        {
            if (currentUser.Identity.IsAuthenticated)
            {
                var userEmail = currentUser.FindFirstValue(ClaimTypes.Email);
                var userEntity = await _userManager.FindByEmailAsync(userEmail);

                var taskEntity = await _taskRepository.GetByIdAsync(id);
                if (userEntity?.Id == taskEntity.CreatedBy.Id)
                {
                    var deleted = await _taskRepository.DeleteAsync(taskEntity);
                    return new BaseResponse<TaskDeleteResponse>
                    {
                        Data = new TaskDeleteResponse
                        {
                            Deleted = deleted
                        }
                    };
                }
                throw new UnauthorizedAccessException("You cannot delete this task, it is not yours.");
            }
            throw new UnauthorizedAccessException("You cannot process to delete this task.");
        }

        public async Task<BaseResponse<TaskListResponse>> GetAll()
        {
            var tasks = await _taskRepository.GetAllTasksAsync();
            var taskModels = _mapper.Map<IList<TaskModel>>(tasks);

            return new BaseResponse<TaskListResponse>
            {
                Data = new TaskListResponse
                {
                    Tasks = taskModels
                }
            };
        }

        public async Task<BaseResponse<TaskListResponse>> GetAllMyTasks(ClaimsPrincipal currentUser)
        {
            var currentUserResponse = await _authService.GetCurrentUser(currentUser);
            var userId = currentUserResponse?.Data?.User?.Id;
            var tasks = await _taskRepository.GetTasksByUserId(userId);
            var taskModels = _mapper.Map<IList<TaskModel>>(tasks);
            return new BaseResponse<TaskListResponse>
            {
                Data = new TaskListResponse
                {
                    Tasks = taskModels
                }
            };
        }

        public async Task<BaseResponse<TaskListResponse>> GetAllTasksOf(TaskOfRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var tasks = await _taskRepository.GetTasksByUserId(user.Id);
                var taskModels = _mapper.Map<IList<TaskModel>>(tasks);
                return new BaseResponse<TaskListResponse>
                {
                    Data = new TaskListResponse
                    {
                        Tasks = taskModels
                    }
                };
            }
            throw new ArgumentException($"User not found with {request.Email}");
        }

        public async Task<BaseResponse<TaskResponse>> GetById(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            var taskModel = _mapper.Map<TaskModel>(task);
            return new BaseResponse<TaskResponse>
            {
                Data = new TaskResponse
                {
                    Task = taskModel
                }
            };
        }

        public async Task<BaseResponse<TaskMarkAsResponse>> MarkAs(TaskMarkAsRequest request, ClaimsPrincipal currentUser)
        {
            if (!Enum.IsDefined(typeof(StatusEnum), request.Status))
            {
                throw new UnauthorizedAccessException($"{request.Status} is not defined as status.");
            }

            if (currentUser.Identity.IsAuthenticated)
            {
                var userEmail = currentUser.FindFirstValue(ClaimTypes.Email);
                var userEntity = await _userManager.FindByEmailAsync(userEmail);

                var status = (StatusEnum)Enum.Parse(typeof(StatusEnum), request.Status);
                var taskEntity = await _taskRepository.GetByIdAsync(request.TaskId);
                var updated = await MarkStatusAs(status, taskEntity, userEntity);
                var updatedModel = _mapper.Map<TaskModel>(updated);
                return new BaseResponse<TaskMarkAsResponse>
                {
                    Data = new TaskMarkAsResponse
                    {
                        Task = updatedModel
                    }
                };
            }
            throw new UnauthorizedAccessException("You cannot change its status.");
        }

        public async Task<BaseResponse<TaskProcessResponse>> Process(int id, ClaimsPrincipal currentUser)
        {
            if (currentUser.Identity.IsAuthenticated)
            {
                var userEmail = currentUser.FindFirstValue(ClaimTypes.Email);
                var userEntity = await _userManager.FindByEmailAsync(userEmail);

                var task = await _taskRepository.GetByIdAsync(id);
                if (task.CreatedBy.Id == userEntity?.Id)
                {
                    if (task.CurrentStatus == StatusEnum.Started || task.CurrentStatus == StatusEnum.Pending)
                    {
                        var updated = await MarkStatusAs(StatusEnum.Doing, task, userEntity);
                        var updatedModel = _mapper.Map<TaskModel>(updated);
                        return new BaseResponse<TaskProcessResponse>
                        {
                            Data = new TaskProcessResponse
                            {
                                Task = updatedModel
                            }
                        };
                    }
                    throw new UnauthorizedAccessException($"You cannot process this task, its status is {task.CurrentStatus}");
                }
                throw new UnauthorizedAccessException("You cannot process this task, you are not assigned to it.");
            }
            throw new UnauthorizedAccessException("You cannot process this task.");
        }

        protected async Task<TaskEntity> MarkStatusAs(StatusEnum status, TaskEntity task, UserEntity updatedBy)
        {
            task.CurrentStatus = status;
            var updated = await _taskRepository.UpdateAsync(task);
            var taskStatus = new TaskStatusEntity
            {
                Status = status,
                Task = task,
                UpdatedBy = updatedBy
            };
            await _taskStatusRepository.AddAsync(taskStatus);
            return updated;
        }

        public BaseResponse<TaskStatusesResponse> GetAllStatus()
        {
            return new BaseResponse<TaskStatusesResponse>
            {
                Data = new TaskStatusesResponse
                {
                    Statuses = Enum.GetNames(typeof(StatusEnum)).ToList()
                }
            };
        }
    }
}

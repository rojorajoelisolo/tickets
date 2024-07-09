using Azure.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tickets.Domain.Requests.Task;
using tickets.Domain.Responses.Auth;
using tickets.Domain.Responses.Base;
using tickets.Domain.Responses.Task;
using tickets.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace tickets.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        private readonly ILogger<TaskController> _logger;

        public TaskController(
            ITaskService taskService,
            ILogger<TaskController> logger
        )
        {
            _taskService = taskService;
            _logger = logger;
        }

        /// <summary>
        /// Tous les tickets.
        /// </summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(BaseResponse<TaskListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAll();
                return Ok(tasks);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[GET] : /task/all : {exception.Message}");
                var errorResponse = new BaseResponse<ErrorResponse>
                {
                    Status = false,
                    Data = new ErrorResponse()
                    {
                        Message = $"An error has occured : {exception.Message}"
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        /// <summary>
        /// Tous mes tickets.
        /// </summary>
        [HttpGet("mine")]
        [ProducesResponseType(typeof(BaseResponse<TaskListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllMyTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllMyTasks(User);
                return Ok(tasks);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[GET] : /task/mine : {exception.Message}");

                var statusCode = exception is UnauthorizedAccessException ?
                                    Unauthorized().StatusCode :
                                    StatusCodes.Status500InternalServerError;
                var errorResponse = new BaseResponse<ErrorResponse>
                {
                    Status = false,
                    Data = new ErrorResponse()
                    {
                        Message = $"An error has occured : {exception.Message}"
                    }
                };
                return StatusCode(statusCode, errorResponse);
            }
        }

        /// <summary>
        /// Tous les tickets d'un utilisateur : par Email.
        /// </summary>
        [HttpPost("user")]
        [ProducesResponseType(typeof(BaseResponse<TaskListResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTasksOf([FromBody] TaskOfRequest request)
        {
            try
            {
                var tasks = await _taskService.GetAllTasksOf(request);
                return Ok(tasks);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[POST] : /task/user : {exception.Message}");

                var statusCode = exception is UnauthorizedAccessException ?
                                    Unauthorized().StatusCode :
                                    StatusCodes.Status500InternalServerError;
                var errorResponse = new BaseResponse<ErrorResponse>
                {
                    Status = false,
                    Data = new ErrorResponse()
                    {
                        Message = $"An error has occured : {exception.Message}"
                    }
                };
                return StatusCode(statusCode, errorResponse);
            }
        }

        /// <summary>
        /// Ticket par ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<TaskResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var task = await _taskService.GetById(id);
                return Ok(task);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[GET] : /task/{id} : {exception.Message}");

                var errorResponse = new BaseResponse<ErrorResponse>
                {
                    Status = false,
                    Data = new ErrorResponse()
                    {
                        Message = $"An error has occured : {exception.Message}"
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        /// <summary>
        /// Créer un ticket.
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(typeof(BaseResponse<TaskCreateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] TaskCreateRequest request)
        {
            try
            {
                var created = await _taskService.Create(request, User);
                return Ok(created);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[POST] : /task/create : {exception.Message}");

                var statusCode = exception is UnauthorizedAccessException ?
                                    Unauthorized().StatusCode :
                                    StatusCodes.Status500InternalServerError;
                var errorResponse = new BaseResponse<ErrorResponse>
                {
                    Status = false,
                    Data = new ErrorResponse()
                    {
                        Message = $"An error has occured : {exception.Message}"
                    }
                };
                return StatusCode(statusCode, errorResponse);
            }
        }

        /// <summary>
        /// Assigner un ticket. (Ajouter des emails)
        /// </summary>
        [HttpPut("assign/{id}")]
        [ProducesResponseType(typeof(BaseResponse<TaskAssignResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Assign(int id, [FromBody] TaskAssignRequest request)
        {
            try
            {
                var task = await _taskService.Assign(id, request, User);
                return Ok(task);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[PUT] : /task/assign/{id} : {exception.Message}");

                var statusCode = exception is UnauthorizedAccessException ?
                                    Unauthorized().StatusCode :
                                    StatusCodes.Status500InternalServerError;
                var errorResponse = new BaseResponse<ErrorResponse>
                {
                    Status = false,
                    Data = new ErrorResponse()
                    {
                        Message = $"An error has occured : {exception.Message}"
                    }
                };
                return StatusCode(statusCode, errorResponse);
            }
        }

        /// <summary>
        /// Supprimer un ticket.
        /// </summary>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(BaseResponse<TaskDeleteResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _taskService.Delete(id, User);
                return Ok(deleted);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[DELETE] : /task/delete/{id} : {exception.Message}");

                var statusCode = exception is UnauthorizedAccessException ?
                                    Unauthorized().StatusCode :
                                    StatusCodes.Status500InternalServerError;
                var errorResponse = new BaseResponse<ErrorResponse>
                {
                    Status = false,
                    Data = new ErrorResponse()
                    {
                        Message = $"An error has occured : {exception.Message}"
                    }
                };
                return StatusCode(statusCode, errorResponse);
            }
        }

        /// <summary>
        /// Traiter un ticket.
        /// </summary>
        [HttpPut("process/{id}")]
        [ProducesResponseType(typeof(BaseResponse<TaskProcessResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Process(int id)
        {
            try
            {
                var processed = await _taskService.Process(id, User);
                return Ok(processed);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[PUT] : /task/process/{id} : {exception.Message}");

                var statusCode = exception is UnauthorizedAccessException ?
                                    Unauthorized().StatusCode :
                                    StatusCodes.Status500InternalServerError;
                var errorResponse = new BaseResponse<ErrorResponse>
                {
                    Status = false,
                    Data = new ErrorResponse()
                    {
                        Message = $"An error has occured : {exception.Message}"
                    }
                };
                return StatusCode(statusCode, errorResponse);
            }
        }

        /// <summary>
        /// Changer le Status d'un ticket.
        /// </summary>
        [HttpPost("change/status")]
        [ProducesResponseType(typeof(BaseResponse<TaskMarkAsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkAs(TaskMarkAsRequest request)
        {
            try
            {
                var updated = await _taskService.MarkAs(request, User);
                return Ok(updated);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[POST] : /task/change/status : {exception.Message}");

                var statusCode = exception is UnauthorizedAccessException ?
                                    Unauthorized().StatusCode :
                                    StatusCodes.Status500InternalServerError;
                var errorResponse = new BaseResponse<ErrorResponse>
                {
                    Status = false,
                    Data = new ErrorResponse()
                    {
                        Message = $"An error has occured : {exception.Message}"
                    }
                };
                return StatusCode(statusCode, errorResponse);
            }
        }

        /// <summary>
        /// Status disponibles des tickets.
        /// </summary>
        [HttpGet("statuses/all")]
        [ProducesResponseType(typeof(BaseResponse<TaskStatusesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllStatus()
        {
            try
            {
                var statuses = _taskService.GetAllStatus();
                return Ok(statuses);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"[GET] : /task/statuses/all : {exception.Message}");

                var errorResponse = new BaseResponse<ErrorResponse>
                {
                    Status = false,
                    Data = new ErrorResponse()
                    {
                        Message = $"An error has occured : {exception.Message}"
                    }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}

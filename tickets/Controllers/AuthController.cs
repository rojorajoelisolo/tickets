using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using tickets.Domain.Requests.Auth;
using tickets.Domain.Responses.Auth;
using tickets.Domain.Responses.Base;
using tickets.Interfaces.Services;
using UserEntity = tickets.Domain.Entities.User;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger
    ) {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Inscription.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(BaseResponse<AuthRegisterResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] AuthRegisterRequest request)
    {
        try
        {
            var registered = await _authService.Register(request);
            return Ok(registered);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[POST] /auth/register  : {exception.Message}");
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
    /// Connexion.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(BaseResponse<AuthRegisterResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequest request)
    {
        try
        {
            var logged = await _authService.Login(request);
            return Ok(logged);

        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[POST] /auth/login : {exception.Message}");

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
    /// A propos de moi.
    /// </summary>
    [HttpGet("aboutme")]
    [ProducesResponseType(typeof(BaseResponse<AuthCurrentUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse<ErrorResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var aboutMe = await _authService.GetCurrentUser(User);
            return Ok(aboutMe);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[GET] /auth/about-me : {exception.Message}");

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
}

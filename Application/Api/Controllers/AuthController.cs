using BibliotecaApi.Application.Api.Auth;
using BibliotecaApi.Application.Api.Auth.DTO;
using BibliotecaApi.Application.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BibliotecaApi.Application.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly JwtTokenService _tokenService;
    private readonly JwtSettings _jwtSettings;

    // Usuário default.
    private const string DefaultUsername = "admin";
    private const string DefaultPassword = "biblioteca123";

    public AuthController(JwtTokenService tokenService, JwtSettings jwtSettings)
    {
        _tokenService = tokenService;
        _jwtSettings = jwtSettings;
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Realiza login e retorna um token JWT")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<LoginOutputDTO>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginInputDTO input)
    {
        if (input.Username != DefaultUsername || input.Password != DefaultPassword)
        {
            return Unauthorized(ApiResponse<LoginOutputDTO>.Falha("Credenciais inválidas."));
        }

        var token = _tokenService.GenerateToken(input.Username);
        var output = new LoginOutputDTO
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes)
        };

        return Ok(ApiResponse<LoginOutputDTO>.Ok(output));
    }
}

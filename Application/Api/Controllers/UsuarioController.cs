using BibliotecaApi.UseCases.Usuario;
using BibliotecaApi.UseCases.Usuario.DTO;
using BibliotecaApi.Application.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BibliotecaApi.Application.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UsuarioController : Controller
{
    private readonly CadastrarUsuarioUC _cadastrarUsuarioUC = new CadastrarUsuarioUC();

    [HttpPost]
    [SwaggerOperation(Summary = "Cadastra um novo usuário retornando o seu respectivo Id")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<int>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarUsuarioInputDTO input)
    {
        try
        {
            int newId = await _cadastrarUsuarioUC.Execute(input);
            return Ok(ApiResponse<int>.Ok(newId));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<int>.Falha("Erro ao cadastrar usuário: " + ex.Message));
        }
    }
}

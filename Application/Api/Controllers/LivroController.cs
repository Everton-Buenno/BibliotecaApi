using BibliotecaApi.Application.Api.Responses;
using BibliotecaApi.UseCases.Livro;
using BibliotecaApi.UseCases.Livro.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BibliotecaApi.Application.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LivroController : Controller
{
    private readonly CadastrarLivroUC _cadastrarLivroUC = new CadastrarLivroUC();
    private readonly ListarLivrosUC _listarLivrosUC = new ListarLivrosUC();

    [HttpPost("cadastrar")]
    [Authorize]
    [SwaggerOperation(Summary = "Adiciona um novo livro retornando o seu respectivo Id")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(ApiResponse<int>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Cadastrar(CadastrarLivroInputDTO input)
    {
        try
        {
            int newId = await _cadastrarLivroUC.Execute(input);
            return Ok(ApiResponse<int>.Ok(newId));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<int>.Falha(ex.Message));
        }
    }

    [HttpGet("listar")]
    [SwaggerOperation(Summary = "Lista todos os livros cadastrados")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<LivroOutputDTO>>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Listar()
    {
        try
        {
            var livros = await _listarLivrosUC.Execute();
            var livrosDto = livros.Select(l => new LivroOutputDTO
            {
                Id = l.Id ?? 0,
                Titulo = l.Titulo,
                Autor = l.Autor,
                ISBN = l.ISBN
            });
            return Ok(ApiResponse<IEnumerable<LivroOutputDTO>>.Ok(livrosDto));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<IEnumerable<LivroOutputDTO>>.Falha(ex.Message));
        }
    }
}

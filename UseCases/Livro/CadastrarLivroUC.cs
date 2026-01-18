using Microsoft.Extensions.Primitives;
using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Livro.DTO;

namespace BibliotecaApi.UseCases.Livro;

public class CadastrarLivroUC
{
    private LivroEntity _livro = new LivroEntity();
    private LivroRepository _repository = new LivroRepository();
    public async Task<int> Execute(CadastrarLivroInputDTO input)
    {
        try
        {
            _livro.Cadastrar(input.Titulo, input.Autor, input.ISBN);

            int idNovoLivro = await _repository.Cadastrar(_livro);
            return idNovoLivro;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao cadastrar livro: " + ex.Message);
        }
        
    }
}

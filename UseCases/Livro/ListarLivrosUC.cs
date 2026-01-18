using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Repositories;

namespace BibliotecaApi.UseCases.Livro;

public class ListarLivrosUC
{
    private readonly LivroRepository _repository = new LivroRepository();

    public async Task<IEnumerable<LivroEntity>> Execute()
    {
        return await _repository.ListarTodosAsync();
    }
}

using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Emprestimo.DTO;

namespace BibliotecaApi.UseCases.Emprestimo;

public class CadastrarEmprestimoUC
{
    private readonly EmprestimoRepository _emprestimoRepository = new EmprestimoRepository();
    private readonly LivroRepository _livroRepository = new LivroRepository();

    public async Task<int> Execute(CadastrarEmprestimoInputDTO input)
    {
        bool livroEmprestado = await _emprestimoRepository.LivroEstaEmprestadoAsync(input.IdLivro);
        if (livroEmprestado)
        {
            throw new Exception("Este livro já está emprestado e ainda não foi devolvido.");
        }

        bool usuarioComAtraso = await _emprestimoRepository.UsuarioPossuiAtrasoAsync(input.IdUsuario);
        if (usuarioComAtraso)
        {
            throw new Exception("Usuário com empréstimo em atraso não pode realizar novo empréstimo.");
        }

        var emprestimo = new EmprestimoEntity();
        emprestimo.Cadastrar(input.IdUsuario, input.IdLivro, input.DataPrevistaDevolucao);

        int idEmprestimo = await _emprestimoRepository.Cadastrar(emprestimo);

        await _livroRepository.MarcarComoIndisponivel(input.IdLivro);

        return idEmprestimo;
    }
}

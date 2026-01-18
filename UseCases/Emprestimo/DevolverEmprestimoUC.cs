using BibliotecaApi.Application.Api.Responses;
using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Repositories;
using BibliotecaApi.UseCases.Emprestimo.DTO;

namespace BibliotecaApi.UseCases.Emprestimo;

public class DevolverEmprestimoUC
{
    private readonly EmprestimoRepository _emprestimoRepository = new EmprestimoRepository();
    private readonly LivroRepository _livroRepository = new LivroRepository();

    public async Task<string> Execute(DevolverEmprestimoInputDTO input)
    {
        var emprestimo = await _emprestimoRepository.ObterPorIdAsync(input.IdEmprestimo);

        if (emprestimo == null)
            throw new Exception("Empréstimo não encontrado.");

        if (emprestimo.DataDevolucao != null)
            throw new Exception("Este empréstimo já foi devolvido.");

        emprestimo.RegistrarDevolucao();

        await _emprestimoRepository.Atualizar(emprestimo);

        await _livroRepository.MarcarComoDisponivel(emprestimo.IdLivro);

        return $"Empréstimo devolvido com sucesso. Multa: R${emprestimo.Multa:F2}, Total a pagar: R${emprestimo.Total:F2}";
    }
}

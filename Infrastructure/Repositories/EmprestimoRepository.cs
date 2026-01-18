using Dapper;
using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Data;
using System.Data.Common;

namespace BibliotecaApi.Infrastructure.Repositories;

public class EmprestimoRepository
{
    private readonly DbSession _session;

    public EmprestimoRepository()
    {
        _session = new DbSession(ConfigurationHelper.GetConfiguration());
    }

    public async Task<int> Cadastrar(EmprestimoEntity emprestimo)
    {
        const string sql = @"
            INSERT INTO Emprestimos (id_usuario, id_livro, data_emprestimo, data_prevista_devolucao, valor)
            VALUES (@id_usuario, @id_livro, @data_emprestimo, @data_prevista_devolucao, @valor)
            RETURNING id;
        ";

        var parameters = new
        {
            id_usuario = emprestimo.IdUsuario,
            id_livro = emprestimo.IdLivro,
            data_emprestimo = emprestimo.DataEmprestimo,
            data_prevista_devolucao = emprestimo.DataPrevistaDevolucao,
            valor = emprestimo.Valor
        };

        using (var connection = _session.Connection)
        {
            var result = await _session.Connection.QueryFirstAsync<int>(sql, parameters);
            return result;
        }
    }

    public async Task<bool> Atualizar(EmprestimoEntity emprestimo)
    {
        const string sql = @"
            UPDATE Emprestimos
            SET id_usuario = @id_usuario,
                id_livro = @id_livro,
                data_emprestimo = @data_emprestimo,
                data_prevista_devolucao = @data_prevista_devolucao,
                data_devolucao = @data_devolucao,
                valor = @valor,
                multa = @multa,
                total = @total
            WHERE id = @id;
        ";
        var parameters = new
        {
            id = emprestimo.Id,
            id_usuario = emprestimo.IdUsuario,
            id_livro = emprestimo.IdLivro,
            data_emprestimo = emprestimo.DataEmprestimo,
            data_prevista_devolucao = emprestimo.DataPrevistaDevolucao,
            data_devolucao = emprestimo.DataDevolucao,
            valor = emprestimo.Valor,
            multa = emprestimo.Multa,
            total = emprestimo.Total
        };
        var rowsAffected = await _session.Connection.ExecuteAsync(sql, parameters);
        return rowsAffected > 0;
    }

    public async Task<EmprestimoEntity?> ObterPorIdAsync(int id)
    {
        var sql = "SELECT id Id, id_usuario IdUsuario, id_livro IdLivro, data_emprestimo DataEmprestimo, data_prevista_devolucao DataPrevistaDevolucao, data_devolucao DataDevolucao, valor Valor, multa Multa, total Total FROM Emprestimos WHERE id = @id";
        return await _session.Connection.QueryFirstOrDefaultAsync<EmprestimoEntity>(sql, new { id });
    }

    public async Task<bool> LivroEstaEmprestadoAsync(int idLivro)
    {
        const string sql = @"
            SELECT COUNT(1) FROM Emprestimos 
            WHERE id_livro = @idLivro AND data_devolucao IS NULL
        ";
        var count = await _session.Connection.ExecuteScalarAsync<int>(sql, new { idLivro });
        return count > 0;
    }

    public async Task<bool> UsuarioPossuiAtrasoAsync(int idUsuario)
    {
        const string sql = @"
            SELECT COUNT(1) FROM Emprestimos 
            WHERE id_usuario = @idUsuario 
            AND data_devolucao IS NULL 
            AND data_prevista_devolucao < @dataAtual
        ";
        var count = await _session.Connection.ExecuteScalarAsync<int>(sql, new { idUsuario, dataAtual = DateTime.Now });
        return count > 0;
    }
}

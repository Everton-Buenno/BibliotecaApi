using Dapper;
using BibliotecaApi.Domain.Entities;
using BibliotecaApi.Infrastructure.Data;

namespace BibliotecaApi.Infrastructure.Repositories;

public class UsuarioRepository
{
    private readonly DbSession _session;

    public UsuarioRepository()
    {
        _session = new DbSession(ConfigurationHelper.GetConfiguration());
    }

    public async Task<int> Cadastrar(UsuarioEntity usuario)
    {
        const string sql = "INSERT INTO Usuarios (nome, cpf, email) VALUES (@nome, @cpf, @email) RETURNING id";

        var parameters = new
        {
            nome = usuario.Nome,
            cpf = usuario.CPF,
            email = usuario.Email
        };

        using (var connection = _session.Connection)
        {
            var result = await _session.Connection.QueryFirstAsync<int>(sql, parameters);
            return result;
        }
    }

    public async Task<bool> ExisteCpfAsync(string cpf)
    {
        const string sql = "SELECT COUNT(1) FROM Usuarios WHERE cpf = @cpf";
        using (var connection = _session.Connection)
        {
            var count = await _session.Connection.ExecuteScalarAsync<int>(sql, new { cpf });
            return count > 0;
        }
    }

    public async Task<UsuarioEntity?> ObterPorIdAsync(int id)
    {
        const string sql = "SELECT id Id, nome Nome, cpf CPF, email Email FROM Usuarios WHERE id = @id";
        using (var connection = _session.Connection)
        {
            return await _session.Connection.QueryFirstOrDefaultAsync<UsuarioEntity>(sql, new { id });
        }
    }
}

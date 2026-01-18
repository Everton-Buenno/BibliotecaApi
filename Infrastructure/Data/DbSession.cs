using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using SQLitePCL;
using System.Data;

namespace BibliotecaApi.Infrastructure.Data;

public sealed class DbSession : IDisposable
{
    private readonly IConfiguration _configuration;
    private Guid _id;
    public IDbConnection Connection { get; }
    public IDbTransaction Transaction { get; set; }

    // Injete IConfiguration via construtor
    public DbSession(IConfiguration configuration)
    {
        Batteries.Init();
        _configuration = configuration;
        _id = Guid.NewGuid();

        // Obtenha a string de conexão do appsettings.json
        var connectionString = _configuration.GetConnectionString("Database");
        Connection = new SqliteConnection(connectionString);
        Connection.Open();
    }

    public void Dispose() => Connection?.Dispose();
}

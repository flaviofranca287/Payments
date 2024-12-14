using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Payments.Infrastructure;

public interface IDatabaseContext : IDisposable
{
    DbConnection Connection { get; }
}

public class DatabaseContext : IDatabaseContext
{
    private readonly object _lock = new();
    private readonly SqlConnection _connection;

    public DbConnection Connection
    {
        get
        {
            OpenConnection(_connection);
            return _connection;
        }
    }

    public DatabaseContext(IOptions<DatabaseSettings> dataBaseSettings)
    {
        _connection = new SqlConnection(dataBaseSettings.Value.ConnectionString);
    }

    private void OpenConnection(SqlConnection conn)
    {
        if (conn.State == ConnectionState.Open) return;
        lock (_lock)
        {
            if (conn.State == ConnectionState.Open) return;
            conn.Open();
        }
    }

    public virtual void Dispose()
        => _connection?.Dispose();
}
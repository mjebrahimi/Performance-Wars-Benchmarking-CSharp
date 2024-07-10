using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace MySqlBulkInsertExcel_Benchmark;

public static class DisableChecksHelper
{
    private const string _disableChecksQuery = """
        SET UNIQUE_CHECKS = 0;
        SET FOREIGN_KEY_CHECKS = 0;
        SET SQL_LOG_BIN = 0;
        """;

    private const string _enableChecksQuery = """
        SET UNIQUE_CHECKS = 1;
        SET FOREIGN_KEY_CHECKS = 1;
        SET SQL_LOG_BIN = 1;
        """;

    public static DisableChecksScope DisableChecks(this DbContext dbContext)
    {
        dbContext.Database.ExecuteSqlRaw(_disableChecksQuery);
        return new DisableChecksScope(dbContext);
    }

    public static async Task<DisableChecksScope> DisableChecksAsync(this DbContext dbContext)
    {
        await dbContext.Database.ExecuteSqlRawAsync(_disableChecksQuery);
        return new DisableChecksScope(dbContext);
    }

    public static DisableChecksScope DisableChecks(this MySqlConnection connection)
    {
        connection.ExecuteNonQuery(_disableChecksQuery);
        return new DisableChecksScope(connection);
    }

    public static async Task<DisableChecksScope> DisableChecksAsync(this MySqlConnection connection)
    {
        await connection.ExecuteNonQueryAsync(_disableChecksQuery);
        return new DisableChecksScope(connection);
    }

    public static DisableChecksScope DisableChecks(this MySql.Data.MySqlClient.MySqlConnection connection)
    {
        connection.ExecuteNonQuery(_disableChecksQuery);
        return new DisableChecksScope(connection);
    }

    public static async Task<DisableChecksScope> DisableChecksAsync(this MySql.Data.MySqlClient.MySqlConnection connection)
    {
        await connection.ExecuteNonQueryAsync(_disableChecksQuery);
        return new DisableChecksScope(connection);
    }

    public class DisableChecksScope : IDisposable, IAsyncDisposable
    {
        private readonly DbContext? _dbContext;
        private readonly MySqlConnection? _connection1;
        private readonly MySql.Data.MySqlClient.MySqlConnection? _connection2;

        public DisableChecksScope(DbContext dbContext) => _dbContext = dbContext;
        public DisableChecksScope(MySqlConnection connection) => _connection1 = connection;
        public DisableChecksScope(MySql.Data.MySqlClient.MySqlConnection connection) => _connection2 = connection;

        public void Dispose()
        {
            if (_dbContext is not null)
                _dbContext.Database.ExecuteSqlRaw(_enableChecksQuery);

            if (_connection1 is not null)
                _connection1.ExecuteNonQuery(_enableChecksQuery);

            if (_connection2 is not null)
                _connection2.ExecuteNonQuery(_enableChecksQuery);
        }

        public async ValueTask DisposeAsync()
        {
            if (_dbContext is not null)
                await _dbContext.Database.ExecuteSqlRawAsync(_enableChecksQuery);

            if (_connection1 is not null)
                await _connection1.ExecuteNonQueryAsync(_enableChecksQuery);

            if (_connection2 is not null)
                await _connection2.ExecuteNonQueryAsync(_enableChecksQuery);
        }
    }
}
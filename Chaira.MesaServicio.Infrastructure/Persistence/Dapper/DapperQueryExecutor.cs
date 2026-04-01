using System.Data;
using System.Data.Common;
using Dapper;
using Chaira.MesaServicio.Application.Common.Interfaces;

namespace Chaira.MesaServicio.Infrastructure.Persistence.Dapper;

public sealed class DapperQueryExecutor : IDapperQueryExecutor
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public DapperQueryExecutor(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null, CancellationToken cancellationToken = default)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        if (connection.State != ConnectionState.Open && connection is DbConnection dbConnection)
        {
            await dbConnection.OpenAsync(cancellationToken);
        }

        return await connection.QueryAsync<T>(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null, CancellationToken cancellationToken = default)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        if (connection.State != ConnectionState.Open && connection is DbConnection dbConnection)
        {
            await dbConnection.OpenAsync(cancellationToken);
        }

        return await connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }
}


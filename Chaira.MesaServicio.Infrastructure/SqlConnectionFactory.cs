using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Chaira.MesaServicio.Application.Common.Interfaces;

namespace Chaira.MesaServicio.Infrastructure;

public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")
            ?? configuration.GetConnectionString("SqlServerConnection")
            ?? throw new InvalidOperationException("SqlServerConnection no configurada");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}


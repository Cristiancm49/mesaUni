using System.Data;

namespace Chaira.MesaServicio.Application.Common.Interfaces;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}


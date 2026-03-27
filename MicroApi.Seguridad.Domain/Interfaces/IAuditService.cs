namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(string action, string entity, int entityId, string userId, object? oldValues = null, object? newValues = null);
        Task LogAsync(string action, string entity, string entityId, string userId, object? oldValues = null, object? newValues = null);
        Task<IEnumerable<object>> GetAuditLogsAsync(string entity, int entityId);
        Task<IEnumerable<object>> GetAuditLogsAsync(string entity, string entityId);
        Task<IEnumerable<object>> GetAuditLogsByUserAsync(string userId);
        Task<IEnumerable<object>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}



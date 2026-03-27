namespace MicroApi.Seguridad.Domain.DTOs.Common
{
    /// <summary>
    /// DTO genérico para respuestas exitosas de la API
    /// </summary>
    /// <typeparam name="T">Tipo de datos de la respuesta</typeparam>
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Crea una respuesta exitosa con datos y mensaje
        /// </summary>
        public static ApiResponseDto<T> SuccessResponse(T data, string message = "OK")
        {
            return new ApiResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Crea una respuesta de error con mensaje
        /// </summary>
        public static ApiResponseDto<T> FailResponse(string message)
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Timestamp = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// DTO para respuestas de error de la API
    /// </summary>
    public class ApiErrorResponseDto
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string? Detail { get; set; }
        public List<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// DTO para respuestas paginadas
    /// </summary>
    /// <typeparam name="T">Tipo de datos de la respuesta</typeparam>
    public class PagedResponseDto<T>
    {
        public List<T> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}

using System.Text.Json;
using FluentValidation;
using Chaira.MesaServicio.Domain.DTOs.Common;

namespace Chaira.MesaServicio.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Error de validacion en {Path}", context.Request.Path);
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Error de validacion", ex.Errors.Select(error => error.ErrorMessage).ToList(), ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado en {Path}", context.Request.Path);
            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, "Ocurrio un error interno en el servidor.", null, ex.Message);
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message, List<string>? errors, string? detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = new ApiErrorResponseDto
        {
            Success = false,
            Message = message,
            Errors = errors,
            Detail = detail,
            Timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}


using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Persons.Handbook.Application.Infrastructure.Extensions;
using Persons.Handbook.Domain.Exceptions;

namespace Persons.Handbook.API.Infrastructure.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
        catch (PersonsHandbookDomainException ex)
        {
            _logger.LogError(ex, ex.Message);

            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (PersonsHandbookDomainNotFoundException ex)
        {
            _logger.LogError(ex, ex.Message);

            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, ex.Message);

            await HandleValidationErrorAsync(context, ex);
        }
        catch (Exception ex)
        {
            const string message = "Something went wrong!";

            _logger.LogError(ex, ex.Message);

            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, message);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(new
        {
            code = statusCode.ToString(),
            message = message
        }.ToJson() ?? string.Empty);
    }

    private async Task HandleValidationErrorAsync(HttpContext context, ValidationException ex)
    {
        var errors = ex.Errors.Select(x => new
        {
            x.PropertyName,
            x.ErrorMessage
        }).ToList();

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await context.Response.WriteAsync(new
        {
            errors
        }.ToJson() ?? string.Empty);
    }
}
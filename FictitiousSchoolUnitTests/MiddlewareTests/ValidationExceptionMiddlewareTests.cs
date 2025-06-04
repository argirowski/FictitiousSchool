using API.Middleware;
using Application.DTOs;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text.Json;
using Xunit;

namespace FictitiousSchoolUnitTests.MiddlewareTests
{
    public class ValidationExceptionMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_NoException_CallsNext()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var nextCalled = false;
            RequestDelegate next = ctx =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };
            var middleware = new ValidationExceptionMiddleware(next);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.True(nextCalled);
            Assert.Equal(200, context.Response.StatusCode); // Default status code
        }

        [Fact]
        public async Task InvokeAsync_ValidationException_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream(); // <-- Add this line

            var failures = new[]
            {
        new ValidationFailure("Prop1", "Error1"),
        new ValidationFailure("Prop2", "Error2")
    };
            var exception = new ValidationException(failures);

            RequestDelegate next = ctx => throw exception;
            var middleware = new ValidationExceptionMiddleware(next);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(400, context.Response.StatusCode);
            Assert.Equal("application/json", context.Response.ContentType);

            context.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();

            var response = JsonSerializer.Deserialize<ValidationErrorResponseDTO>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(response);
            Assert.Equal(2, response.Errors.Count);
            Assert.Contains(response.Errors, e => e.PropertyName == "Prop1" && e.ErrorMessage == "Error1");
            Assert.Contains(response.Errors, e => e.PropertyName == "Prop2" && e.ErrorMessage == "Error2");
        }
    }
}
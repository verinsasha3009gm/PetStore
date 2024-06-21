

using PetStore.Products.Domain.Enum;
using PetStore.Products.Domain.Result;

namespace PetStore.Products.API.Middlewares
{
    public class ExceptionHanlingMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger _logger;
        public ExceptionHanlingMiddleware(RequestDelegate next /*ILogger logger*/)
        {
            _next = next;
            //_logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandlerExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandlerExceptionAsync(HttpContext httpContext, Exception exception)
        {
            //_logger.Error(exception, exception.Message);
            var errorMessage = exception.Message;
            var responce = exception switch
            {
                UnauthorizedAccessException _=> new BaseResult() 
                { ErrorMessage = errorMessage,ErrorCode =(int)ErrorCodes.InternalServerException },
                _ => new BaseResult() 
                { ErrorMessage = exception.Message, ErrorCode = (int)ErrorCodes.InternalServerException},
            };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)responce.ErrorCode;
            await httpContext.Response.WriteAsJsonAsync(responce);
        }
    }
}

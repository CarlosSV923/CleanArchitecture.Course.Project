using CleanArchitecture.Course.Project.Application.Abstractions.Messaging;
using CleanArchitecture.Course.Project.Domain.Entities.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace CleanArchitecture.Course.Project.Application.Abstractions.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IBaseRequest
    where TResponse : Result
    {
        
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;

            try{
                _logger.LogInformation("Handling Request {name} {request}", name, request);
                var result = await next();
                if(result.IsSucceeded){
                    _logger.LogInformation("Request {name} handled successfully", name);
                }
                else{
                    using(LogContext.PushProperty("Error", result.Error)){
                        _logger.LogError("Request {name} handled with errors", name);
                    }
                }
                return result;
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error handling request {name} {request}", name, request);
                throw;
            }
        }
    }
}
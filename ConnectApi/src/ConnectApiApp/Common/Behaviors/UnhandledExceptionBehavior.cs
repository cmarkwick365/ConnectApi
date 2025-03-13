using MediatR;
using Serilog;

namespace ConnectApiApp.Common.Behaviors
{
    public class UnhandledExceptionBehavior<TRequest, TResponse>(ILogger logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Log any unhandled exceptions from api requests (https://garywoodfine.com/how-to-use-mediatr-pipeline-behaviours/)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="next"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                logger.Error(ex, "ConnectApi: Unhandled Exception for Request {Name} {@Request}", requestName, request);

                throw;
            }
        }
    }
}

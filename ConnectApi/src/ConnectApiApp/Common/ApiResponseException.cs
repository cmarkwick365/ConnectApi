using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace ConnectApiApp.Common
{
    

    public class ApiResponseException : Exception
    {
        public ApiResponseException(ILogger logger)
        {
            
        }
        public ApiResponseException(ILogger logger, Exception e, string message)
            : base(message)
        {
            var reference = Guid.NewGuid().ToString();
            logger.Error(e, $"{message}. Ref: {reference}");
            throw new ApiException(
                $"{message} Error reference: {reference}",
                StatusCodes.Status500InternalServerError);
        }

    }
}

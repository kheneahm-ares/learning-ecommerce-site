namespace Catalog.Features.Shared
{
    public static class ServiceResultExtensions
    {
        public static IResult ToHttp<T>(this ServiceResult<T> serviceResult)
        {
            if (serviceResult.IsSuccessful)
            {
                return Results.Ok(serviceResult.Data);
            }
            else
            {
                return serviceResult.ErrorType switch
                {
                    ErrorType.NotFound => Results.NotFound(serviceResult.ErrorMessage),
                    ErrorType.BadRequest => Results.BadRequest(serviceResult.ErrorMessage),
                    ErrorType.Conflict => Results.Conflict(serviceResult.ErrorMessage),
                    ErrorType.Unauthorized => Results.Unauthorized(),
                    _ => Results.StatusCode(500)
                };
            }
        }
    }
}

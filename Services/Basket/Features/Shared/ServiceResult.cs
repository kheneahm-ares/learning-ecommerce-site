namespace Basket.Features.Shared
{
    public class ServiceResult<T>
    {
        public T Data { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public ErrorType ErrorType { get; set; }

        public ServiceResult() { }

        public void Success(T data)
        {
            Data = data;
            IsSuccessful = true;
            ErrorMessage = null;
        }

        public void Failure(string errorMessage, ErrorType type)
        {
            IsSuccessful = false;
            ErrorMessage = errorMessage;
            ErrorType = type;
        }
    }

    public class ServiceResult
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public ErrorType ErrorType { get; set; }

        public void Failure(string errorMessage, ErrorType type)
        {
            IsSuccessful = false;
            ErrorMessage = errorMessage;
            ErrorType = type;
        }
    }

    public enum ErrorType
    {
        NotFound,
        BadRequest,
        Conflict,
        Unauthorized
    }
}

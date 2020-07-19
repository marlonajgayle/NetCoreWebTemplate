namespace NetCoreWebTemplate.Api.Contracts.Version1.Responses
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }

        public Response(T result, string message)
        {
            IsSuccess = true;
            Message = message;
            Result = result;
        }
    }
}

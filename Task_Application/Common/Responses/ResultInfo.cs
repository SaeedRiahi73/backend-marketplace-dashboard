namespace Task_Application.Common.Responses
{
    public class ResultInfo<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public IEnumerable<string>? Errors { get; set; }

        public static ResultInfo<T> Success(T data, string message = "")
        {
            return new ResultInfo<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        public static ResultInfo<T> Failure(IEnumerable<string> errors, string message = "")
        {
            return new ResultInfo<T>
            {
                IsSuccess = false,
                Errors = errors,
                Message = message
            };
        }
    }
}

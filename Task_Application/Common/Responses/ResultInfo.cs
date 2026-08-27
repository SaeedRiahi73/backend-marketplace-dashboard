namespace Task_Application.Common.Responses
{
    using Task_Application.Enums;

    public class ResultInfo<T>
    {
        public bool IsSuccess { get; set; }

        public ResultStatus Status { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public IEnumerable<string>? Errors { get; set; }

        public static ResultInfo<T> Success(
            T data,
            string message = "",
            ResultStatus status = ResultStatus.Ok)
        {
            return new ResultInfo<T>
            {
                IsSuccess = true,
                Status = status,
                Data = data,
                Message = message
            };
        }

        public static ResultInfo<T> Failure(
            IEnumerable<string> errors,
            string message = "",
            ResultStatus status = ResultStatus.BadRequest)
        {
            return new ResultInfo<T>
            {
                IsSuccess = false,
                Status = status,
                Errors = errors,
                Message = message
            };
        }
    }
}

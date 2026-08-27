namespace Task_Application.Enums
{
    public enum ResultStatus
    {
        Ok = 200,
        Created = 201,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        ValidationError = 422,
        TooManyRequests = 429,
        Error = 500
    }
}

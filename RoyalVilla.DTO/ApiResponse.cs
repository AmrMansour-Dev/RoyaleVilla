namespace RoyalVilla.DTO
{
    public class ApiResponse<TData>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public TData? Data { get; set; }
        public object? Errors { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;


        public static ApiResponse<TData> Create(bool success, int statusCode, string message, TData? data = default, object? errors = null)
        {
            return new ApiResponse<TData>
            {
                Success = success,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = errors
            };
        }

        //Success
        public static ApiResponse<TData> Ok(string Message,TData data)
        {
            return Create(true, 200, Message, data);
        }

        public static ApiResponse<TData> CreatedAt(string Message, TData data)
        {
            return Create(true, 201, Message, data);
        }
        public static ApiResponse<TData> NoContent(string Message = "Operation Completed Successfully")
        {
            return Create(true, 204, Message);
        }

        //Client Errors
        public static ApiResponse<TData> BadRequest(string Message, object? Errors = null)
        {
            return Create(false, 400, Message,errors:Errors);
        }
        public static ApiResponse<TData> NotFound(string Message = "Resource Not Found")
        {
            return Create(false, 404, Message);
        }
        public static ApiResponse<TData> Conflict(string Message)
        {
            return Create(false, 409, Message);
        }

        //Generic Errors
        public static ApiResponse<TData> Error(int statuscode,string Message,object? Errors = null)
        {
            return Create(false, statuscode, Message,errors: Errors);
        }
    }
}

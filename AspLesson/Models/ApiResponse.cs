namespace AspLesson.Models;

/// <summary>
/// 统一 API 响应结构，所有接口均返回 HTTP 200，通过 code 区分业务状态。
/// </summary>
public class ApiResponse<T>
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Success")
        => new() { Code = 200, Message = message, Data = data };

    public static ApiResponse<T> Fail(IDictionary<string, string[]> errors, string message = "Validation failed")
        => new() { Code = 400, Message = message, Errors = errors };

    public static ApiResponse<T> Fail(string message)
        => new() { Code = 400, Message = message };
}

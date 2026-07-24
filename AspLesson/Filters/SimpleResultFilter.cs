using AspLesson.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspLesson.Filters;

public class SimpleResultFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult
            && objectResult.StatusCode is null or 200
            && objectResult.Value is not ApiResponse<object>)
        {
            var original = objectResult.Value;

            objectResult.Value = new ApiResponse<object>
            {
                Code = 200,
                Message = "Success",
                Data = original
            };
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {

    }
}
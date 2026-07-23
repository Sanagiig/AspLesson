using AspLesson.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspLesson.Filters;

/// <summary>
/// 自动验证 Action Filter：
/// 从 DI 获取对应的 IValidator&lt;T&gt;，验证请求参数，
/// 验证失败时返回 HTTP 200 + 统一 ApiResponse（含字段错误信息）。
/// </summary>
public class ValidationActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 从 Action 参数中找到复杂类型（非基元类型）的请求对象
        var requestArg = context.ActionArguments.Values
            .FirstOrDefault(v => v is not null && v.GetType() is { IsPrimitive: false, IsEnum: false } type
                                               && type != typeof(string));

        if (requestArg is not null)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(requestArg.GetType());

            if (context.HttpContext.RequestServices.GetService(validatorType) is IValidator validator)
            {
                var validationContext = new ValidationContext<object>(requestArg);
                var result = await validator.ValidateAsync(validationContext);

                if (!result.IsValid)
                {
                    var errors = result.ToDictionary();
                    context.Result = new OkObjectResult(ApiResponse<object>.Fail(errors));
                    return;
                }
            }
        }

        await next();
    }
}
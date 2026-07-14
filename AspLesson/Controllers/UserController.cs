using AspLesson.Data.Requests;
using AspLesson.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspLesson.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    [HttpPost]
    public ActionResult<ApiResponse<UserRegistrationRequest>> RegisterUser([FromBody] UserRegistrationRequest request)
    {
        // 验证由 ValidationActionFilter 自动处理，此处只需关注业务逻辑
        return Ok(ApiResponse<UserRegistrationRequest>.Success(request, "User registered successfully"));
    }
}

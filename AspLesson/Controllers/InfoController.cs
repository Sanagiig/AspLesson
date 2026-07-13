using AspLesson.exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AspLesson.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class InfoController:ControllerBase
{
    public string GetInfo()
    {
        return "This is an ASP.NET Core application.";
    }

    public string GetException()
    {
        throw new NotFoundException("GetException","test");
    }
}
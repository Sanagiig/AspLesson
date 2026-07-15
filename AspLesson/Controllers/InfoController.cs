using AspLesson.exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AspLesson.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class InfoController:ControllerBase
{
    [HttpGet]
    public string GetInfo()
    {
        return "This is an ASP.NET Core application.";
    }

    [HttpGet]
    public string GetException()
    {
        throw new NotFoundException("GetException","test");
    }
    
    [HttpGet]
    public string GetException2()
    {
        throw new Exception("GetException2");
    }
}
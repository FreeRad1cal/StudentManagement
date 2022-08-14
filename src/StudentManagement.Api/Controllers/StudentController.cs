using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly ILogger<StudentController> _logger;

    public StudentController(ILogger<StudentController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "TODO")]
    public IActionResult Get()
    {
        return Ok();
    }
}

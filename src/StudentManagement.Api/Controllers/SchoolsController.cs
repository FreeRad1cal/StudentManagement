using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models;
using StudentManagement.Core.Interfaces;

namespace StudentManagement.Api.Controllers;

[ApiController]
[Route("api")]
public class SchoolsController : ControllerBase
{
    private readonly ILogger<StudentsController> _logger;
    private readonly ISchoolService _schoolService;

    public SchoolsController(ILogger<StudentsController> logger, ISchoolService schoolService)
    {
        _logger = logger;
        _schoolService = schoolService;
    }

    [HttpGet("schools")]
    public async Task<IActionResult> GetSchoolsAsync([FromQuery] GetSchoolsRequestDto request)
    {
        var result = await _schoolService.FindAsync(request.Name);

        return Ok(result);
    }
}

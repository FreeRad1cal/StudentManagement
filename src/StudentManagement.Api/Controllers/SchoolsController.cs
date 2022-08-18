using System.Net;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models;
using StudentManagement.Core.Interfaces;
using StudentManagement.Core.Models;

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
    [ProducesResponseType(typeof(IEnumerable<SchoolDto>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetSchoolsAsync([FromQuery] GetSchoolsRequest request)
    {
        var result = await _schoolService.FindAsync(request.Name);

        return Ok(result);
    }
}

using System.Net;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models;
using StudentManagement.Core.Interfaces;
using StudentManagement.Core.Models;

namespace StudentManagement.Api.Controllers;

[ApiController]
[Route("api")]
public class StudentsController : ControllerBase
{
    private readonly ILogger<StudentsController> _logger;
    private readonly IStudentService _studentService;

    public StudentsController(ILogger<StudentsController> logger, IStudentService studentService)
    {
        _logger = logger;
        _studentService = studentService;
    }

    [HttpGet("students")]
    [ProducesResponseType(typeof(IEnumerable<StudentDto>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetStudentsAsync([FromQuery] GetStudentsRequest request)
    {
        var result = await _studentService.FindAsync(request.FirstName, request.LastName, request.DateOfBirth,
            request.SchoolId);

        return Ok(result);
    }
    
    [HttpGet("students/{studentId:int}")]
    [ProducesResponseType(typeof(StudentDto), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetStudentById([FromRoute] int studentId)
    {
        var result = await _studentService.FindByIdAsync(studentId);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    
    [HttpGet("students/{studentId:int}/grades")]
    [ProducesResponseType(typeof(IEnumerable<GradeDto>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetStudentGrades([FromRoute] int studentId)
    {
        var result = await _studentService.FindGradesAsync(studentId);

        return Ok(result);
    }
}

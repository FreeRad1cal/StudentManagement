using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Models;
using StudentManagement.Core.Interfaces;

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
    public async Task<IActionResult> GetStudentsAsync([FromQuery] GetStudentsRequestDto request)
    {
        var result = await _studentService.FindAsync(request.FirstName, request.LastName, request.DateOfBirth,
            request.SchoolId);

        return Ok(result);
    }
    
    [HttpGet("students/{studentId:int}")]
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
    public async Task<IActionResult> GetStudentGrades([FromRoute] int studentId)
    {
        var result = await _studentService.FindGradesAsync(studentId);

        return Ok(result);
    }
}

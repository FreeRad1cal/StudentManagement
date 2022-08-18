using Microsoft.AspNetCore.Mvc;
using StudentDataImporter.Api.Models;
using StudentDataImporter.Api.Services;

namespace StudentDataImporter.Api.Controllers;

[ApiController]
[Route("api")]
public class DataImportController : ControllerBase
{
    private readonly ILogger<DataImportController> _logger;
    private readonly IDataImporter _dataImporter;

    public DataImportController(ILogger<DataImportController> logger, IDataImporter dataImporter)
    {
        _logger = logger;
        _dataImporter = dataImporter;
    }

    [HttpPost("student-data")]
    public async Task<IActionResult> ImportStudentDataAsync([FromForm] ImportDataRequest request)
    {
        if (request.StudentData.Length == 0)
        {
            _logger.LogError("Empty file submitted");
            return BadRequest("A nonempty file is required");
        }

        await using var stream = request.StudentData.OpenReadStream();
        var response = await _dataImporter.ImportData(stream);
        
        return Ok(response);
    }
}

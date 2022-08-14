using Microsoft.AspNetCore.Mvc;
using StudentDataImporter.Api.Models;
using StudentDataImporter.Api.Services;

namespace StudentDataImporter.Api.Controllers;

[ApiController]
[Route("api")]
public class DataImportController : ControllerBase
{
    private readonly ILogger<DataImportController> _logger;
    private readonly IDataImporterService _dataImporterService;

    public DataImportController(ILogger<DataImportController> logger, IDataImporterService dataImporterService)
    {
        _logger = logger;
        _dataImporterService = dataImporterService;
    }

    [HttpPost(Name = "student-data")]
    public async Task<IActionResult> ImportStudentDataAsync([FromForm] ImportDataRequest request)
    {
        if (request.StudentData.Length == 0)
        {
            return BadRequest("A nonempty file is required");
        }

        string data;
        using (var stream = new StreamReader(request.StudentData.OpenReadStream()))
        {
            data = await stream.ReadToEndAsync();
        }
        
        var result = await _dataImporterService.ImportData(data);
        
        return Ok(result);
    }
}

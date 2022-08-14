using System.ComponentModel.DataAnnotations;

namespace StudentDataImporter.Api.Models;

public class ImportDataRequest
{
    [Required]
    public IFormFile StudentData { get; set; }
}
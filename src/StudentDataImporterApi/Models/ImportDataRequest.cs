using System.ComponentModel.DataAnnotations;

namespace StudentDataImporterApi.Models;

public class ImportDataRequest
{
    [Required]
    public IFormFile StudentData { get; set; }
}
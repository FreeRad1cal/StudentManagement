using System.Text.Json.Serialization;

namespace StudentManagement.Core.Models;

public class SchoolDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DistrictDto District { get; set; }
}
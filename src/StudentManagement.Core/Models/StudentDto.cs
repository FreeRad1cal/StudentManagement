using System.Text.Json.Serialization;
using StudentManagement.Core.Entities;

namespace StudentManagement.Core.Models;

public class StudentDto
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SchoolDto School { get; set; }
}
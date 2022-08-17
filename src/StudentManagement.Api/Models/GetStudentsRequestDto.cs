namespace StudentManagement.Api.Models;

public class GetStudentsRequestDto
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }
    
    public int? SchoolId { get; set; }
}

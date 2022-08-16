namespace StudentDataImporter.Api.DataAccess.Entities;

public class SchoolEnrollment
{
    public int Id { get; set; }
    
    public int StudentId { get; set; }
    
    public int SchoolId { get; set; }
}
namespace StudentDataImporter.Api.DataAccess.Entities;

public class Course
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Year { get; set; }
    
    public int SchoolId { get; set; }
}
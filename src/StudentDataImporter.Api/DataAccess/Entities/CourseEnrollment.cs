namespace StudentDataImporter.Api.DataAccess.Entities;

public class CourseEnrollment
{
    public int Id { get; set; }
    
    public int StudentId { get; set; }
    
    public int CourseId { get; set; }
}
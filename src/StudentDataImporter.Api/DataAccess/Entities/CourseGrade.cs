namespace StudentDataImporter.Api.DataAccess.Entities;

public class CourseGrade
{
    public int Id { get; set; }
    
    public int CourseId { get; set; }
    
    public int StudentId { get; set; }
    
    public string LetterGrade { get; set; }
}
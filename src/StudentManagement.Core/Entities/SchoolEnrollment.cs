namespace StudentManagement.Core.Entities;

public class SchoolEnrollment
{
    public int Id { get; set; }
    
    public int StudentId { get; set; }
    
    public Student Student { get; set; }
    
    public int SchoolId { get; set; }
    
    public School School { get; set; }
}
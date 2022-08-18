namespace StudentManagement.Core.Entities;

public class CourseEnrollment: IEntity
{
    public int Id { get; set; }
    
    public int StudentId { get; set; }
    
    public Student Student { get; set; }
    
    public int CourseId { get; set; }
    
    public Course Course { get; set; }
}
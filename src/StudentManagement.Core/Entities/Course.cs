namespace StudentManagement.Core.Entities;

public class Course: IEntity
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Year { get; set; }
    
    public SchoolYear SchoolYear { get; set; }
    
    public int SchoolId { get; set; }
    
    public School School { get; set; }
    
    public IEnumerable<CourseEnrollment> CourseEnrollments { get; set; }
}
namespace StudentManagement.Core.Entities;

public class Student: IEntity
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string MiddleName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public Gender Gender { get; set; }
    
    public IEnumerable<CourseEnrollment> CourseEnrollments { get; set; }
    
    public SchoolEnrollment SchoolEnrollment { get; set; }
}
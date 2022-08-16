namespace StudentDataImporter.Api.DataAccess.Entities;

public class Student
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string MiddleName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public Gender Gender { get; set; }
}
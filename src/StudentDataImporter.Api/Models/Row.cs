using StudentDataImporter.Api.DataAccess.Entities;

namespace StudentDataImporter.Api.Models;

public class Row
{
    public District District { get; set; }
    
    public School School { get; set; }
    
    public Student Student { get; set; }
    
    public SchoolYear SchoolYear { get; set; }
    
    public Course Course { get; set; }
    
    public Grade Grade { get; set; }
}
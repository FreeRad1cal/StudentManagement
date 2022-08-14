namespace StudentManagement.Core.Entities;

public class School
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int DistrictId { get; set; }
    
    public District District { get; set; }
}
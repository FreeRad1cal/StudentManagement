namespace StudentDataImporter.Api.Models;

public class DataImporterResponse
{
    public int DistrictsImported { get; set; }
    
    public int SchoolsImported { get; set; }
    
    public int StudentsImported { get; set; }
    
    public int CoursesImported { get; set; }
    
    public int CourseGradesImported { get; set; }
}
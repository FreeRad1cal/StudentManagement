namespace Core.Models;

public class DataImporterResponse
{
    public int RecordsImported { get; set; }
    
    public int DuplicatesSkipped { get; set; }
}
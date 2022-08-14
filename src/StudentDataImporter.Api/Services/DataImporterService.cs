using StudentDataImporter.Api.Models;

namespace StudentDataImporter.Api.Services;

public class DataImporterService: IDataImporterService
{
    public DataImporterService()
    {
        
    }
    
    public async Task<DataImporterResponse> ImportData(string data)
    {
        throw new NotImplementedException();
    }
}
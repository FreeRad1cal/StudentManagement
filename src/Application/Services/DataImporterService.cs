using Core.Interfaces;
using Core.Models;

namespace Application.Services;

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
using StudentDataImporter.Api.Models;

namespace StudentDataImporter.Api.Services;

public interface IDataImporterService
{
    Task<DataImporterResponse> ImportData(string data);
}
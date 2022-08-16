using StudentDataImporter.Api.Models;

namespace StudentDataImporter.Api.Services;

public interface IDataImporter
{
    Task<DataImporterResponse> ImportData(Stream data);
}
using Core.Models;

namespace Core.Interfaces;

public interface IDataImporterService
{
    Task<DataImporterResponse> ImportData(string data);
}
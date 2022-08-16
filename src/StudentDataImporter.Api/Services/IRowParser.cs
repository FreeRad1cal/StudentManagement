using StudentDataImporter.Api.Models;

namespace StudentDataImporter.Api.Services;

public interface IRowParser
{
    Row ParseRow(string row);
}
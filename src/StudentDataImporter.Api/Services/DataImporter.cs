using StudentDataImporter.Api.Configuration;
using StudentDataImporter.Api.DataAccess.Entities;
using StudentDataImporter.Api.DataAccess.Repositories;
using StudentDataImporter.Api.Models;

namespace StudentDataImporter.Api.Services;

public class DataImporter: IDataImporter
{
    private readonly ILogger<DataImporter> _logger;
    private readonly IRowParser _rowParser;
    private readonly ConfigurationConstants _constants;
    private readonly IRepository<District> _districtRepository;

    public DataImporter(ILogger<DataImporter> logger, IRowParser rowParser, ConfigurationConstants constants, IRepository<District> districtRepository)
    {
        _logger = logger;
        _rowParser = rowParser;
        _constants = constants;
        _districtRepository = districtRepository;
    }
    
    public async Task<DataImporterResponse> ImportData(Stream data)
    {
        var reader = new StreamReader(data);
        await reader.ReadLineAsync();
        var batchSize = _constants.ImportBatchSize;

        var response = new DataImporterResponse();
        
        while (!reader.EndOfStream)
        {
            var rows = (await GetBatchOfRowsAsync(reader, batchSize)).ToList();

            var districtsImported = await _districtRepository.BulkInsertOrUpdateAsync(rows.Select(r => r.District).ToList());
            response.DistrictsImported += districtsImported;
            // foreach (var row in rows)
            // {
            //     row.School.DistrictId = row.District.Id;
            // }
            //
            // await _dbContext.BulkInsertOrUpdateAsync(rows.Select(row => row.School).ToList());
            // foreach (var row in rows)
            // {
            //     row.Course.SchoolId = row.School.Id;
            // }
            //
            // await _dbContext.BulkInsertOrUpdateAsync(rows.Select(row => row.Course).ToList(), config => config);
            // var courseEnrollments = rows.Select(row => new CourseEnrollment
            //     { CourseId = row.Course.Id, StudentId = row.Student.Id });


        }

        return response;
    }

    private async Task<IEnumerable<Row>> GetBatchOfRowsAsync(StreamReader reader, int batchSize)
    {
        var rows = new List<Row>();
        for (var i = 0; i < batchSize && !reader.EndOfStream; i++)
        {
            var line = await reader.ReadLineAsync() ?? string.Empty;
            rows.Add(_rowParser.ParseRow(line));
        }

        return rows;
    }
}
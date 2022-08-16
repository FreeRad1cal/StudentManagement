using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using StudentDataImporter.Api.DataAccess.Entities;

namespace StudentDataImporter.Api.DataAccess.Repositories;

public class SchoolRepository: IRepository<School>
{
    private readonly IConfiguration _configuration;

    public SchoolRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<int> BulkInsertOrUpdateAsync(ICollection<School> items)
    {
        var connectionString = _configuration.GetConnectionString("StudentManagementDb");
        
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(@"
            DROP TYPE IF EXISTS dbo.TVP_School;
            CREATE TYPE dbo.TVP_School AS TABLE
            (
                Name nvarchar(1000),
                DistrictId int
            );
        ");
        
        const string sql = @$"
            MERGE INTO dbo.Schools as target
            USING (SELECT Name, DistrictId from @source) as source
            ON target.Name = source.Name AND target.DistrictId = source.DistrictId
            WHEN MATCHED THEN
                UPDATE SET target.Name = target.Name
            WHEN NOT MATCHED THEN
                INSERT (Name, DistrictId)
                VALUES (source.Name, source.DistrictId)
            OUTPUT
                $action as action,
                inserted.*;
            DROP TYPE TVP_School;
            ";
        var dataTable = new DataTable();
        dataTable.Columns.Add("Name");
        dataTable.Columns.Add("DistrictId");
        foreach (var item in items.DistinctBy(d => (d.Name, d.DistrictId)))
        {
            dataTable.Rows.Add(item.Name, item.DistrictId);
        }
        
        var response = await connection.QueryAsync(sql, new { 
            source = dataTable.AsTableValuedParameter("TVP_School") 
        });

        var responseDict = response
            .Cast<IDictionary<string, object>>()
            .ToDictionary(r => (r["Name"], r["DistrictId"]), r => (r["Id"], r["action"]));
        foreach (var item in items)
        {
            if (responseDict[(item.Name, item.DistrictId)].Item1 is int id)
            {
                item.Id = id;
            }
        }

        return GetInsertedCount(responseDict);
    }

    private static int GetInsertedCount(IDictionary<(object, object), (object, object)> responseDict) => 
        responseDict.Values.Count(v => v.Item2.Equals("INSERT"));
}
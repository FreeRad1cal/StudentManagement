using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using StudentDataImporter.Api.DataAccess.Entities;

namespace StudentDataImporter.Api.DataAccess.Repositories;

public class DistrictRepository: IRepository<District>
{
    private readonly IConfiguration _configuration;

    public DistrictRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<int> BulkInsertOrUpdateAsync(ICollection<District> items)
    {
        var connectionString = _configuration.GetConnectionString("StudentManagementDb");
        
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(@"
            DROP TYPE IF EXISTS dbo.TVP_District;
            CREATE TYPE dbo.TVP_District AS TABLE
            (
                Name varchar(1000)
            );
        ");
        
        const string sql = @$"
            MERGE INTO dbo.Districts as target
            USING (SELECT Name from @source) as source
            ON target.Name = source.Name
            WHEN MATCHED THEN
                UPDATE SET target.Name = target.Name
            WHEN NOT MATCHED THEN
                INSERT (Name)
                VALUES (source.Name)
            OUTPUT
                $action as action,
                inserted.*;
            DROP TYPE TVP_District;
            ";
        var dataTable = new DataTable();
        dataTable.Columns.Add("Name");
        foreach (var item in items.DistinctBy(d => d.Name))
        {
            dataTable.Rows.Add(item.Name);
        }
        
        var response = await connection.QueryAsync(sql, new { 
            source = dataTable.AsTableValuedParameter("TVP_District") 
        });

        var responseDict = response
            .Cast<IDictionary<string, object>>()
            .ToDictionary(r => r["Name"], r => (r["Id"], r["action"]));
        foreach (var item in items)
        {
            if (responseDict[item.Name].Item1 is int id)
            {
                item.Id = id;
            }
        }

        return GetInsertedCount(responseDict);
    }

    private static int GetInsertedCount(IDictionary<object, (object, object)> responseDict) => 
        responseDict.Values.Count(v => v.Item2.Equals("INSERT"));
}
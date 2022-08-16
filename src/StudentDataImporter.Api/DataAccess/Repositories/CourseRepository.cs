using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using StudentDataImporter.Api.DataAccess.Entities;

namespace StudentDataImporter.Api.DataAccess.Repositories;

public class CourseRepository: IRepository<Course>
{
    private readonly IConfiguration _configuration;

    public CourseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<int> BulkInsertOrUpdateAsync(ICollection<Course> items)
    {
        var connectionString = _configuration.GetConnectionString("StudentManagementDb");
        
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(@"
            DROP TYPE IF EXISTS dbo.TVP_Course;
            CREATE TYPE dbo.TVP_Course AS TABLE
            (
                Name nvarchar(1000),
                SchoolId int,
                Year nvarchar(1000)
            );
        ");
        
        const string sql = @$"
            MERGE INTO dbo.Courses as target
            USING (SELECT Name, SchoolId, Year from @source) as source
            ON target.Name = source.Name AND target.SchoolId = source.SchoolId AND target.Year = source.Year
            WHEN MATCHED THEN
                UPDATE SET target.Name = target.Name
            WHEN NOT MATCHED THEN
                INSERT (Name, SchoolId, Year)
                VALUES (source.Name, source.SchoolId, source.Year)
            OUTPUT
                $action as action,
                inserted.*;
            DROP TYPE TVP_Course;
            ";
        var dataTable = new DataTable();
        dataTable.Columns.Add("Name");
        dataTable.Columns.Add("SchoolId");
        dataTable.Columns.Add("Year");
        foreach (var item in items.DistinctBy(d => (d.Name, d.SchoolId, d.Year)))
        {
            dataTable.Rows.Add(item.Name, item.SchoolId, item.Year);
        }
        
        var response = await connection.QueryAsync(sql, new { 
            source = dataTable.AsTableValuedParameter("TVP_Course") 
        });

        var responseDict = response
            .Cast<IDictionary<string, object>>()
            .ToDictionary(r => (r["Name"], r["SchoolId"], r["Year"]), r => (r["Id"], r["action"]));
        foreach (var item in items)
        {
            if (responseDict[(item.Name, item.SchoolId, item.Year)].Item1 is int id)
            {
                item.Id = id;
            }
        }

        return GetInsertedCount(responseDict);
    }

    private static int GetInsertedCount(IDictionary<(object, object, object), (object, object)> responseDict) => 
        responseDict.Values.Count(v => v.Item2.Equals("INSERT"));
}
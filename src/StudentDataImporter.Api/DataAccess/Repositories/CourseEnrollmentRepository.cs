using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using StudentDataImporter.Api.DataAccess.Entities;

namespace StudentDataImporter.Api.DataAccess.Repositories;

public class CourseEnrollmentRepository: IRepository<CourseEnrollment>
{
    private readonly IConfiguration _configuration;

    public CourseEnrollmentRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<int> BulkInsertOrUpdateAsync(ICollection<CourseEnrollment> items)
    {
        var connectionString = _configuration.GetConnectionString("StudentManagementDb");
        
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(@"
            DROP TYPE IF EXISTS dbo.TVP_CourseEnrollment;
            CREATE TYPE dbo.TVP_CourseEnrollment AS TABLE
            (
                StudentId int,
                CourseId int
            );
        ");
        
        const string sql = @$"
            MERGE INTO dbo.CourseEnrollments as target
            USING (SELECT StudentId, CourseId from @source) as source
            ON target.StudentId = source.StudentId AND target.CourseId = source.CourseId
            WHEN MATCHED THEN
                UPDATE SET target.StudentId = target.StudentId
            WHEN NOT MATCHED THEN
                INSERT (StudentId, CourseId)
                VALUES (source.StudentId, source.CourseId)
            OUTPUT
                $action as action,
                inserted.*;
            DROP TYPE TVP_CourseEnrollment;
            ";
        var dataTable = new DataTable();
        dataTable.Columns.Add("StudentId");
        dataTable.Columns.Add("CourseId");
        foreach (var item in items.DistinctBy(d => (d.StudentId, d.CourseId)))
        {
            dataTable.Rows.Add(item.StudentId, item.CourseId);
        }
        
        var response = await connection.QueryAsync(sql, new { 
            source = dataTable.AsTableValuedParameter("TVP_CourseEnrollment") 
        });

        var responseDict = response
            .Cast<IDictionary<string, object>>()
            .ToDictionary(r => (r["StudentId"], r["CourseId"]), r => (r["Id"], r["action"]));
        foreach (var item in items)
        {
            if (responseDict[(item.StudentId, item.CourseId)].Item1 is int id)
            {
                item.Id = id;
            }
        }

        return GetInsertedCount(responseDict);
    }

    private static int GetInsertedCount(IDictionary<(object, object), (object, object)> responseDict) => 
        responseDict.Values.Count(v => v.Item2.Equals("INSERT"));
}
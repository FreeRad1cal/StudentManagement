using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using StudentDataImporter.Api.DataAccess.Entities;

namespace StudentDataImporter.Api.DataAccess.Repositories;

public class SchoolEnrollmentRepository: IRepository<SchoolEnrollment>
{
    private readonly IConfiguration _configuration;

    public SchoolEnrollmentRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<int> BulkInsertOrUpdateAsync(ICollection<SchoolEnrollment> items)
    {
        var connectionString = _configuration.GetConnectionString("StudentManagementDb");
        
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(@"
            DROP TYPE IF EXISTS dbo.TVP_SchoolEnrollment;
            CREATE TYPE dbo.TVP_SchoolEnrollment AS TABLE
            (
                StudentId int,
                SchoolId int
            );
        ");
        
        const string sql = @$"
            MERGE INTO dbo.SchoolEnrollments as target
            USING (SELECT StudentId, SchoolId from @source) as source
            ON target.StudentId = source.StudentId AND target.SchoolId = source.SchoolId
            WHEN MATCHED THEN
                UPDATE SET target.StudentId = target.StudentId
            WHEN NOT MATCHED THEN
                INSERT (StudentId, SchoolId)
                VALUES (source.StudentId, source.SchoolId)
            OUTPUT
                $action as action,
                inserted.*;
            DROP TYPE TVP_SchoolEnrollment;
            ";
        var dataTable = new DataTable();
        dataTable.Columns.Add("StudentId");
        dataTable.Columns.Add("SchoolId");
        foreach (var item in items.DistinctBy(d => (d.StudentId, d.SchoolId)))
        {
            dataTable.Rows.Add(item.StudentId, item.SchoolId);
        }
        
        var response = await connection.QueryAsync(sql, new { 
            source = dataTable.AsTableValuedParameter("TVP_SchoolEnrollment") 
        });

        var responseDict = response
            .Cast<IDictionary<string, object>>()
            .ToDictionary(r => (r["StudentId"], r["SchoolId"]), r => (r["Id"], r["action"]));
        foreach (var item in items)
        {
            if (responseDict[(item.StudentId, item.SchoolId)].Item1 is int id)
            {
                item.Id = id;
            }
        }

        return GetInsertedCount(responseDict);
    }

    private static int GetInsertedCount(IDictionary<(object, object), (object, object)> responseDict) => 
        responseDict.Values.Count(v => v.Item2.Equals("INSERT"));
}
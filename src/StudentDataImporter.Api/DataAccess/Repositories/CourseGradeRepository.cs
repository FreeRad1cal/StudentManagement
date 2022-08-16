using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using StudentDataImporter.Api.DataAccess.Entities;

namespace StudentDataImporter.Api.DataAccess.Repositories;

public class CourseGradeRepository: IRepository<CourseGrade>
{
    private readonly IConfiguration _configuration;

    public CourseGradeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<int> BulkInsertOrUpdateAsync(ICollection<CourseGrade> items)
    {
        var connectionString = _configuration.GetConnectionString("StudentManagementDb");
        
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(@"
            DROP TYPE IF EXISTS dbo.TVP_CourseGrade;
            CREATE TYPE dbo.TVP_CourseGrade AS TABLE
            (
                StudentId int,
                CourseId int,
                LetterGrade nvarchar(2)
            );
        ");
        
        const string sql = @$"
            MERGE INTO dbo.CourseGrades as target
            USING (SELECT StudentId, CourseId, LetterGrade from @source) as source
            ON target.StudentId = source.StudentId AND target.CourseId = source.CourseId 
            WHEN MATCHED THEN
                UPDATE SET target.LetterGrade = target.LetterGrade
            WHEN NOT MATCHED THEN
                INSERT (StudentId, CourseId, LetterGrade)
                VALUES (source.StudentId, source.CourseId, source.LetterGrade)
            OUTPUT
                $action as action,
                inserted.*;
            DROP TYPE TVP_CourseGrade;
            ";
        var dataTable = new DataTable();
        dataTable.Columns.Add("StudentId");
        dataTable.Columns.Add("CourseId");
        dataTable.Columns.Add("LetterGrade");
        foreach (var item in items.DistinctBy(d => (d.StudentId, d.CourseId)))
        {
            dataTable.Rows.Add(item.StudentId, item.CourseId, item.LetterGrade);
        }
        
        var response = await connection.QueryAsync(sql, new { 
            source = dataTable.AsTableValuedParameter("TVP_CourseGrade") 
        });

        var responseDict = response
            .Cast<IDictionary<string, object>>()
            .ToDictionary(r => (r["StudentId"], r["CourseId"], r["LetterGrade"]), r => (r["Id"], r["action"]));
        foreach (var item in items)
        {
            if (responseDict[(item.StudentId, item.CourseId, item.LetterGrade)].Item1 is int id)
            {
                item.Id = id;
            }
        }

        return GetInsertedCount(responseDict);
    }

    private static int GetInsertedCount(IDictionary<(object, object, object), (object, object)> responseDict) => 
        responseDict.Values.Count(v => v.Item2.Equals("INSERT"));
}
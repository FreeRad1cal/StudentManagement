using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using StudentDataImporter.Api.DataAccess.Entities;

namespace StudentDataImporter.Api.DataAccess.Repositories;

public class StudentRepository: IRepository<Student>
{
    private readonly IConfiguration _configuration;

    public StudentRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<int> BulkInsertOrUpdateAsync(ICollection<Student> items)
    {
        var connectionString = _configuration.GetConnectionString("StudentManagementDb");
        
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(@"
            DROP TYPE IF EXISTS dbo.TVP_Student;
            CREATE TYPE dbo.TVP_Student AS TABLE
            (
                FirstName nvarchar(1000),
                LastName nvarchar(1000),
                MiddleName nvarchar(1000),
                DateOfBirth Date,
                Gender nvarchar(1)
            );
        ");
        
        const string sql = @$"
            MERGE INTO dbo.Students as target
            USING (SELECT FirstName, LastName, MiddleName, DateOfBirth, Gender from @source) as source
            ON target.FirstName = source.FirstName AND target.LastName = source.LastName AND target.DateOfBirth = source.DateOfBirth
            WHEN MATCHED THEN
                UPDATE SET target.FirstName = target.FirstName
            WHEN NOT MATCHED THEN
                INSERT (FirstName, LastName, MiddleName, DateOfBirth, Gender)
                VALUES (source.FirstName, source.LastName, source.MiddleName, source.DateOfBirth, source.Gender)
            OUTPUT
                $action as action,
                inserted.*;
            DROP TYPE TVP_Student;
            ";
        var dataTable = new DataTable();
        dataTable.Columns.Add("FirstName");
        dataTable.Columns.Add("LastName");
        dataTable.Columns.Add("MiddleName");
        dataTable.Columns.Add("DateOfBirth");
        dataTable.Columns.Add("Gender");
        foreach (var item in items.DistinctBy(d => (d.FirstName, d.LastName, d.DateOfBirth)))
        {
            dataTable.Rows.Add(item.FirstName, item.LastName, item.MiddleName, item.DateOfBirth, item.Gender);
        }
        
        var response = await connection.QueryAsync(sql, new { 
            source = dataTable.AsTableValuedParameter("TVP_Student") 
        });

        var responseDict = response
            .Cast<IDictionary<string, object>>()
            .ToDictionary(r => (r["FirstName"], r["LastName"], r["DateOfBirth"]), r => (r["Id"], r["action"]));
        foreach (var item in items)
        {
            if (responseDict[(item.FirstName, item.LastName, item.DateOfBirth)].Item1 is int id)
            {
                item.Id = id;
            }
        }

        return GetInsertedCount(responseDict);
    }

    private static int GetInsertedCount(IDictionary<(object, object, object), (object, object)> responseDict) => 
        responseDict.Values.Count(v => v.Item2.Equals("INSERT"));
}
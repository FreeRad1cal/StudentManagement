using System.Text.RegularExpressions;
using StudentDataImporter.Api.DataAccess.Entities;
using StudentDataImporter.Api.Models;

namespace StudentDataImporter.Api.Services;

public class RowParser: IRowParser
{
    public Row ParseRow(string row)
    {
        var columns = row.Split(',');
        if (columns.Length != 8)
        {
            throw new ArgumentException("Invalid row format");
        }

        var (firstName, middleName, lastName) = ParseName(columns[0]);
        
        return new Row
        {
            District = new District
            {
                Name = columns[7]
            },
            Course = new Course
            {
                Name = columns[1],
                Year = $"20{columns[4]}"
            },
            Grade = new Grade
            {
                LetterValue = columns[3]
            },
            School = new School
            {
                Name = columns[6]
            },
            SchoolYear = new SchoolYear
            {
                Year = $"20{columns[4]}"
            },
            Student = new Student
            {
                DateOfBirth = DateTime.Parse(columns[2]),
                FirstName = firstName ?? throw new ArgumentException("Invalid name format"),
                MiddleName = middleName,
                LastName = lastName ?? throw new ArgumentException("Invalid name format"),
                Gender = Enum.Parse<Gender>(columns[5])
            }
        };
    }

    private (string FirstName, string MiddleName, string LastName) ParseName(string name)
    {
        var nameRegex = new Regex(@"^(\w+)\s(?:(\w+).?\s)?(\w+)$");
        var matches = nameRegex.Matches(name);
        if (!matches.Any())
        {
            throw new ArgumentException("Invalid name format");
        }

        var groups = matches.First().Groups;

        return (
            string.IsNullOrWhiteSpace(groups[1].Value.Trim()) ? null : groups[1].Value.Trim(),
            string.IsNullOrWhiteSpace(groups[2].Value.Trim()) ? null : groups[2].Value.Trim(),
            string.IsNullOrWhiteSpace(groups[3].Value.Trim()) ? null : groups[3].Value.Trim()
        );
    }
}
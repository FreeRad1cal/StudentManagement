using StudentManagement.Core.Models;

namespace StudentManagement.Core.Interfaces;

public interface ISchoolService
{
    Task<IEnumerable<SchoolDto>> FindAsync(string? name);
}
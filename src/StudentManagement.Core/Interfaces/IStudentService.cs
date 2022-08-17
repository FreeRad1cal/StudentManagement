using StudentManagement.Core.Models;

namespace StudentManagement.Core.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> FindAsync(string firstName, string lastName, DateTime? dateOfBirth, int? schoolId);

    Task<StudentDto> FindByIdAsync(int studentId);

    Task<IEnumerable<GradeDto>> FindGradesAsync(int studentId);
}
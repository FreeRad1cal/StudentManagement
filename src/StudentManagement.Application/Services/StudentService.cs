using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces;
using StudentManagement.Core.Models;

namespace StudentManagement.Application.Services;

public class StudentService: IStudentService
{
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<CourseGrade> _gradeRepository;

    public StudentService(IRepository<Student> studentRepository, IRepository<CourseGrade> gradeRepository)
    {
        _studentRepository = studentRepository;
        _gradeRepository = gradeRepository;
    }
    
    public async Task<IEnumerable<StudentDto>> FindAsync(string firstName, string lastName, DateTime? dateOfBirth, int? schoolId)
    {
        var query = _studentRepository.Get()
            .Include(s => s.SchoolEnrollment)
            .ThenInclude(e => e.School)
            .AsQueryable();

        if (firstName != null)
        {
            query = query.Where(s => s.FirstName == firstName);
        }

        if (lastName != null)
        {
            query = query.Where(s => s.LastName == lastName); 
        }

        if (dateOfBirth != null)
        {
            query = query.Where(s => s.DateOfBirth == dateOfBirth); 
        }
        
        if (schoolId != null)
        {
            query = query.Where(s => s.SchoolEnrollment.School.Id == schoolId);
        }

        var students = await query.AsNoTracking().ToListAsync();

        return students.Select(s => new StudentDto
        {
            Id = s.Id,
            FirstName = s.FirstName,
            MiddleName = s.MiddleName,
            LastName = s.LastName,
            DateOfBirth = s.DateOfBirth,
            Gender = s.Gender.ToString(),
            School = new SchoolDto
            {
                Id = s.SchoolEnrollment.School.Id,
                Name = s.SchoolEnrollment.School.Name
            }
        });
    }

    public async Task<StudentDto> FindByIdAsync(int studentId)
    {
        var student = await _studentRepository.Get()
            .Where(s => s.Id == studentId)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return student == null
            ? null
            : new StudentDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender.ToString(),
                School = new SchoolDto
                {
                    Id = student.SchoolEnrollment.School.Id,
                    Name = student.SchoolEnrollment.School.Name
                }
            };
    }

    public async Task<IEnumerable<GradeDto>> FindGradesAsync(int studentId)
    {
        var grades = await _gradeRepository.Get()
            .Where(g => g.StudentId == studentId)
            .Include(g => g.Course)
            .ThenInclude(c => c.School)
            .AsNoTracking()
            .ToListAsync();

        return grades.Select(g => new GradeDto
        {
            CourseName = g.Course.Name,
            SchoolName = g.Course.School.Name,
            Grade = g.LetterGrade
        });
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentManagement.Core.Entities;

namespace StudentManagement.Infrastructure.Data;

public class StudentContextSeed
{
    private readonly StudentDbContext _context;
    private readonly ILogger<StudentContextSeed> _logger;

    public StudentContextSeed(StudentDbContext context, ILogger<StudentContextSeed> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task MigrateDatabase()
    {
        _logger.LogInformation("Migrating database...");
        await _context.Database.MigrateAsync();
    }
    
    public async Task SeedAsync()
    {
        _logger.LogInformation("Seeding letter grades...");
        if (!await _context.Grades.AnyAsync())
        {
            var preconfiguredGrades = GetPreconfiguredLetterGrades();
            await _context.Grades.AddRangeAsync(preconfiguredGrades);
            await _context.SaveChangesAsync();
        }
        
        _logger.LogInformation("Seeding school years...");
        if (!await _context.SchoolYears.AnyAsync())
        {
            var preconfiguredSchoolYears = GetPreconfiguredSchoolYears();
            await _context.SchoolYears.AddRangeAsync(preconfiguredSchoolYears);
            await _context.SaveChangesAsync();
        }
    }

    private static IEnumerable<Grade> GetPreconfiguredLetterGrades() =>
        new[] { "A+", "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D", "D-", "F" }
            .Select(value => new Grade { LetterValue = value });
    
    private static IEnumerable<SchoolYear> GetPreconfiguredSchoolYears() =>
        Enumerable.Range(2000, 23)
            .Select(year => new SchoolYear { Year = year.ToString()});
}
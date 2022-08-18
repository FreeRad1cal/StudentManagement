using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces;
using StudentManagement.Core.Models;

namespace StudentManagement.Application.Services;

public class SchoolService: ISchoolService
{
    private readonly IRepository<School> _schoolRepository;

    public SchoolService(IRepository<School> schoolRepository)
    {
        _schoolRepository = schoolRepository;
    }
    
    public async Task<IEnumerable<SchoolDto>> FindAsync(string? name)
    {
        var query = _schoolRepository.Get();

        if (name != null)
        {
            query = query.Where(s => s.Name == name);
        }

        var schools = await query.ToListAsync();

        return schools.Select(s => new SchoolDto
        {
            Id = s.Id,
            Name = s.Name,
            District = new DistrictDto
            {
                Id = s.District.Id,
                Name = s.District.Name
            }
        });
    }
}
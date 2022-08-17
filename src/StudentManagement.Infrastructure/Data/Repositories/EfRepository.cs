using StudentManagement.Core.Interfaces;

namespace StudentManagement.Infrastructure.Data.Repositories;

public class EfRepository<T>: IRepository<T> where T: class
{
    private readonly StudentDbContext _context;

    public EfRepository(StudentDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<T> Get() => 
        _context.Set<T>();

    public async Task<T> GetByIdAsync(int id) => 
        await _context.Set<T>().FindAsync(id);
}
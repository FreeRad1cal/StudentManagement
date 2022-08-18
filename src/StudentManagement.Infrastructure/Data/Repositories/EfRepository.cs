using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces;

namespace StudentManagement.Infrastructure.Data.Repositories;

public class EfRepository<T>: IRepository<T> where T: class, IEntity
{
    private readonly StudentDbContext _context;

    public EfRepository(StudentDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<T> Get()
    {
        return _context.Set<T>().AsNoTracking();
    }
    
    public IQueryable<T> Get(IEnumerable<string> includes)
    {
        return JoinRelatedEntities(Get(), includes);
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var query = _context.Set<T>().AsNoTracking();

        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }
    
    public async Task<T> GetByIdAsync(int id, IEnumerable<string> includes)
    {
        var query = _context.Set<T>().AsNoTracking();
        JoinRelatedEntities(query, includes);

        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }

    private static IQueryable<T> JoinRelatedEntities(IQueryable<T> query, IEnumerable<string> includes) => 
        includes.Aggregate(query, (current, include) => current.Include(include));
}
using StudentManagement.Core.Entities;

namespace StudentManagement.Core.Interfaces;

public interface IRepository<T> where T: class, IEntity
{
    IQueryable<T> Get();
    
    IQueryable<T> Get(IEnumerable<string> includes);

    Task<T> GetByIdAsync(int id);
    
    Task<T> GetByIdAsync(int id, IEnumerable<string> includes);
}
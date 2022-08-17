namespace StudentManagement.Core.Interfaces;

public interface IRepository<T> where T: class
{
    IQueryable<T> Get();

    Task<T> GetByIdAsync(int id);
}
namespace StudentDataImporter.Api.DataAccess.Repositories;

public interface IRepository<T> where T: class
{
    Task<int> BulkInsertOrUpdateAsync(ICollection<T> items);
}
using BookManagementAPI.Infrastructure.Entities;

namespace BookManagementAPI.Infrastructure.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task<bool> DeleteAsync(int id);
}

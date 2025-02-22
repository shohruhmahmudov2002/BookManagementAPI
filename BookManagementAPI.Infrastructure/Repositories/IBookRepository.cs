using BookManagementAPI.Infrastructure.Entities;

namespace BookManagementAPI.Infrastructure.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task AddAsync(Book book);
    Task AddRangeAsync(IEnumerable<Book> books);
    Task UpdateAsync(Book book);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<string>> GetPopularBooks(int pageNumber, int pageSize);
}

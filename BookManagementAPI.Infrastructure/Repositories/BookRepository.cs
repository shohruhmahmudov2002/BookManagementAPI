using Microsoft.EntityFrameworkCore;
using BookManagementAPI.Infrastructure.Entities;
using BookManagementAPI.Infrastructure.Data;

namespace BookManagementAPI.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BookContext _context;

    public BookRepository(BookContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books.Where(b => !b.IsDeleted).ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) 
        { 
            return null; 
        }

        book.ViewsCount += 1;
        await _context.SaveChangesAsync();

        return book;
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Book> books)
    {
        await _context.Books.AddRangeAsync(books);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if(book  == null || book.IsDeleted) 
        { 
            return false; 
        }

        book.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<string>> GetPopularBooks(int pageNumber, int pageSize)
    {
        return await _context.Books
            .Where(b => !b.IsDeleted)
            .OrderByDescending(b => b.ViewsCount)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(b => b.Title)
            .ToListAsync();
    }
}

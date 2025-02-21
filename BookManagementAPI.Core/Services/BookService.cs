using BookManagementAPI.Core.DTOs;
using BookManagementAPI.Core.Interfaces;
using BookManagementAPI.Infrastructure.Data;
using BookManagementAPI.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagementAPI.Core.Services;

public class BookService : IBookService
{
    private readonly BookContext _context;

    public BookService(BookContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<string>> GetPopularBooks(int pageNumber, int pageSize)
    {
        var titles = await _context.Books
            .Where(b => !b.IsDeleted) 
            .OrderByDescending(b => b.ViewsCount) 
            .Skip((pageNumber - 1) * pageSize) 
            .Take(pageSize)
            .Select(b => b.Title)
            .ToListAsync();

        return titles;
    }

    public async Task<BookDto?> GetById(int id)
    {
        var book = await _context.Books.Where(b => !b.IsDeleted).FirstOrDefaultAsync(b => b.Id == id);
        if (book == null) return null;

        book.ViewsCount += 1; 
        await _context.SaveChangesAsync();

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            PublicationYear = book.PublicationYear,
            ViewsCount = book.ViewsCount
        };
    }

    public async Task<bool> AddSingle(BookCreateDto bookDto)
    {
        var exists = await _context.Books.AnyAsync(b => b.Title == bookDto.Title);
        if (exists) return false;

        var book = new Book
        {
            Title = bookDto.Title,
            Author = bookDto.Author,
            PublicationYear = bookDto.PublicationYear,
            ViewsCount = 0
        };

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(List<BookDto> addedBooks, List<string> skippedTitles)> AddBulk(List<BookCreateDto> bookDtos)
    {
        var existingTitles = await _context.Books
            .Where(b => bookDtos.Select(d => d.Title).Contains(b.Title))
            .Select(b => b.Title)
            .ToListAsync();

        var newBooks = bookDtos
            .Where(dto => !existingTitles.Contains(dto.Title))
            .Select(dto => new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                PublicationYear = dto.PublicationYear,
                ViewsCount = 0
            })
            .ToList();

        List<BookDto> addedBookDtos = new();

        if (newBooks.Any())
        {
            await _context.Books.AddRangeAsync(newBooks);
            await _context.SaveChangesAsync();

            addedBookDtos = newBooks.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                PublicationYear = b.PublicationYear,
                ViewsCount = b.ViewsCount
            }).ToList();
        }

        return (addedBookDtos, existingTitles);
    }

    public async Task<bool> Update(int id, BookUpdateDto bookDto)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        book.Title = bookDto.Title;
        book.Author = bookDto.Author;
        book.PublicationYear = bookDto.PublicationYear;

        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSingle(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        book.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(List<int> deletedIds, List<int> notFoundIds)> DeleteBulk(List<int> bookIds)
    {
        var books = await _context.Books
            .Where(b => bookIds.Contains(b.Id) && !b.IsDeleted)
            .ToListAsync();

        var foundIds = books.Select(b => b.Id).ToList();
        var notFoundIds = bookIds.Except(foundIds).ToList();

        if (foundIds.Any())
        {
            books.ForEach(b => b.IsDeleted = true);
            await _context.SaveChangesAsync();
        }

        return (foundIds, notFoundIds);
    }

    public async Task<bool> Restore(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null || !book.IsDeleted) return false;

        book.IsDeleted = false;
        await _context.SaveChangesAsync();
        return true;
    }
}

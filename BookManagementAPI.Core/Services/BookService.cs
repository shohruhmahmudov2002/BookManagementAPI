using BookManagementAPI.Core.DTOs;
using BookManagementAPI.Core.Interfaces;
using BookManagementAPI.Infrastructure.Entities;
using BookManagementAPI.Infrastructure.Repositories;

namespace BookManagementAPI.Core.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<string>> GetPopularBooks(int pageNumber, int pageSize)
    {
        return await _bookRepository.GetPopularBooks(pageNumber, pageSize);
    }

    public async Task<BookDto?> GetById(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
        { 
            return null; 
        }

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
        var exists = await _bookRepository.GetAllAsync();
        if (exists.Any(b => b.Title == bookDto.Title)) 
        { 
            return false; 
        }

        var book = new Book
        {
            Title = bookDto.Title,
            Author = bookDto.Author,
            PublicationYear = bookDto.PublicationYear,
            ViewsCount = 0
        };

        await _bookRepository.AddAsync(book);
        return true;
    }

    public async Task<(List<BookDto> addedBooks, List<string> skippedTitles)> AddBulk(List<BookCreateDto> bookDtos)
    {
        var existingBooks = await _bookRepository.GetAllAsync();
        var existingTitles = existingBooks.Select(b => b.Title).ToHashSet();

        var skippedTitles = bookDtos
            .Where(dto => existingTitles.Contains(dto.Title))
            .Select(dto => dto.Title)
            .ToList();

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

        if (newBooks.Any())
        {
            await _bookRepository.AddRangeAsync(newBooks);
        }

        var addedBooks = newBooks.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            PublicationYear = b.PublicationYear,
            ViewsCount = b.ViewsCount
        }).ToList();

        return (addedBooks, skippedTitles);

    }

    public async Task<bool> Update(int id, BookUpdateDto bookDto)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) 
        { 
            return false; 
        }

        book.Title = bookDto.Title;
        book.Author = bookDto.Author;
        book.PublicationYear = bookDto.PublicationYear;

        await _bookRepository.UpdateAsync(book);
        return true;
    }

    public async Task<bool> DeleteSingle(int id)
    {
        return await _bookRepository.DeleteAsync(id);
    }

    public async Task<(List<int> deletedIds, List<int> notFoundIds)> DeleteBulk(List<int> bookIds)
    {
        var deletedIds = new List<int>();
        var notFoundIds = new List<int>();

        foreach(var id in bookIds)
        {
            var success = await _bookRepository.DeleteAsync(id);
            if(success)
            {
                deletedIds.Add(id);
            }
            else
            {
                notFoundIds.Add(id);
            }
        }

        return (deletedIds, notFoundIds);
    }

    public async Task<bool> Restore(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null || !book.IsDeleted) 
        { 
            return false; 
        }

        book.IsDeleted = false;
        await _bookRepository.UpdateAsync(book);
        return true;
    }
}

using BookManagementAPI.Core.DTOs;

namespace BookManagementAPI.Core.Interfaces;

public interface IBookService
{
    Task<IEnumerable<string>> GetPopularBooks(int pageNumber, int pageSize);
    Task<BookDto?> GetById(int id);
    Task<bool> AddSingle(BookCreateDto bookDto);
    Task<(List<BookDto> addedBooks, List<string> skippedTitles)> AddBulk(List<BookCreateDto> bookDtos);
    Task<bool> Update(int id, BookUpdateDto bookDto);
    Task<bool> DeleteSingle(int id);
    Task<(List<int> deletedIds, List<int> notFoundIds)> DeleteBulk(List<int> bookIds);
    Task<bool> Restore(int id);
}

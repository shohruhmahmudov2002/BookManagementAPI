using BookManagementAPI.Api.Examples;
using BookManagementAPI.Core.DTOs;
using BookManagementAPI.Core.Interfaces;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace BookManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("popular-books")]
    public async Task<IActionResult> GetPopularBooks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var books = await _bookService.GetPopularBooks(page, pageSize);
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _bookService.GetById(id);
        return book != null ? Ok(book) : NotFound();
    }

    [HttpPost("add-single")]
    [SwaggerRequestExample(typeof(BookCreateDto),typeof(CreateBookExample))]
    public async Task<IActionResult> AddBook([FromBody] BookCreateDto bookDto)
    {
        var success = await _bookService.AddSingle(bookDto);
        return success ? CreatedAtAction(nameof(GetBook), new { id = bookDto.Title }, bookDto) : BadRequest("Book already exists.");
    }

    [HttpPost("add-bulk")]
    [SwaggerRequestExample(typeof(BookCreateDto), typeof(CreateBooksExample))]
    public async Task<IActionResult> AddBooks([FromBody] List<BookCreateDto> bookDtos)
    {
        var (addedBooks, skippedTitles) = await _bookService.AddBulk(bookDtos);

        return Ok(new
        {
            Message = $"{addedBooks.Count} books added, {skippedTitles.Count} books skipped.",
            AddedBooks = addedBooks,
            SkippedBooks = skippedTitles
        });
    }


    [HttpPut("{id}")]
    [SwaggerRequestExample(typeof(BookUpdateDto), typeof(UpdateBookExample))]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateDto bookDto)
    {
        var success = await _bookService.Update(id, bookDto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var success = await _bookService.DeleteSingle(id);
        return success ? NoContent() : NotFound();
    }

    [HttpPost("delete-bulk")]
    public async Task<IActionResult> DeleteBooks([FromBody] List<int> bookIds)
    {
        var (deletedIds, notFoundIds) = await _bookService.DeleteBulk(bookIds);

        return Ok(new
        {
            Message = "Bulk delete completed.",
            DeletedBooks = deletedIds,
            NotFoundBooks = notFoundIds
        });
    }

    [HttpPut("{id}/restore")]
    public async Task<IActionResult> RestoreBook(int id)
    {
        var success = await _bookService.Restore(id);
        return success ? Ok("Book restored successfully") : NotFound();
    }
}

using BookManagementAPI.Core.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace BookManagementAPI.Api.Examples;

public class CreateBookExample : IExamplesProvider<BookCreateDto>
{
    public BookCreateDto GetExamples()
    {
        return new()
        {
            Title = "Harry Potter 7",
            Author ="J.K.Rowling",
            PublicationYear = 1998
        };
    }
}
public class CreateBooksExample : IExamplesProvider<IEnumerable<BookCreateDto>>
{
    public IEnumerable<BookCreateDto> GetExamples()
    {
        return new List<BookCreateDto>
        {
            new BookCreateDto
            {
                Title = "Harry Potter 7",
                Author ="J.K.Rowling",
                PublicationYear = 1998
            },
            new BookCreateDto
            {
                Title = "Harry Potter 7",
                Author ="J.K.Rowling",
                PublicationYear = 1998
            },
        };
    }

}

public class UpdateBookExample : IExamplesProvider<BookUpdateDto>
{
    public BookUpdateDto GetExamples()
    {
        return new()
        {
            Title = "Harry Potter 7",
            Author = "J.K.Rowling",
            PublicationYear = 1998
        };
    }
}



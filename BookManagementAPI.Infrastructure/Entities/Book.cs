namespace BookManagementAPI.Infrastructure.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int ViewsCount { get; set; } = 0;
    public bool IsDeleted { get; set; } = false;
}

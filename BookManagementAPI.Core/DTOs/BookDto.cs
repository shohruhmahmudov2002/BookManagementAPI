namespace BookManagementAPI.Core.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int ViewsCount { get; set; }

    public double PopularityScore => (ViewsCount * 0.5) + ((DateTime.Now.Year - PublicationYear) * 2);
}

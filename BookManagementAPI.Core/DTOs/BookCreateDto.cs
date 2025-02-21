using System.ComponentModel.DataAnnotations;

namespace BookManagementAPI.Core.DTOs;

public class BookCreateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Author { get; set; } = string.Empty;

    [Range(1000, 2025, ErrorMessage = "Publication year must be between 1000 and 2025")]
    public int PublicationYear { get; set; }
}

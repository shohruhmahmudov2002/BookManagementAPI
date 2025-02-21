using System.Reflection;
using BookManagementAPI.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagementAPI.Infrastructure.Data;

public class BookContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public BookContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

using BookManagementAPI.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookManagementAPI.Core.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired();

        builder.Property(x => x.Author).IsRequired();

        builder.Property(x => x.PublicationYear).IsRequired();
    }
}

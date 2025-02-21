using BookManagementAPI.Infrastructure.Data;
using BookManagementAPI.Core.Services;
using Microsoft.EntityFrameworkCore;
using BookManagementAPI.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BookContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IBookService, BookService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

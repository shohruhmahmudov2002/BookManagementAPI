using BookManagementAPI.Infrastructure.Data;
using BookManagementAPI.Core.Services;
using Microsoft.EntityFrameworkCore;
using BookManagementAPI.Core.Interfaces;
using BookManagementAPI.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BookContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IBookRepository, BookRepository>();
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

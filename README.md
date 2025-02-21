# Book Management API

## Overview

Book Management API is a RESTful web service built with **ASP.NET Core Web API** and **Entity Framework Core**, allowing users to manage books by adding, updating, deleting (soft delete), restoring, and retrieving books.

## Features

-  **CRUD Operations** (Create, Read, Update, Delete) for books
-  **Soft Delete** (Marks books as deleted instead of removing them)
-  **Restore Deleted Books**
-  **Get Popular Books** (Based on ViewsCount with pagination)
-  **Bulk Add & Bulk Delete Support**
-  **Pagination & Sorting**
-  **Swagger API Documentation**

## Technologies Used

- **C#**
- **.NET 8 / ASP.NET Core Web API**
- **Entity Framework Core** (EF Core)
- **SQL Server**
- **Swagger (Swashbuckle)**

## Project Structure

```
BookManagementAPI/
│── BookManagementAPI.Api/          # API Layer (Controllers)
│── BookManagementAPI.Core/         # Business Logic Layer (Services, DTOs, Interfaces)
│── BookManagementAPI.Infrastructure/ # Data Access Layer (DbContext, Entities, Repositories)
│── README.md                       # Documentation
│── .gitignore                       # Git Ignore File
```

## Setup & Installation

### Clone the Repository

```sh
git clone https://github.com/yourusername/BookManagementAPI.git
cd BookManagementAPI
```

### Configure Database Connection

Modify `appsettings.json` in **BookManagementAPI.Api**:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=BookManagementDb;Trusted_Connection=True;"
}
```

### Apply Migrations & Update Database

```sh
cd BookManagementAPI.Api
# Run migration commands
dotnet ef migrations add InitialCreate --project ../BookManagementAPI.Infrastructure --startup-project .
dotnet ef database update --project ../BookManagementAPI.Infrastructure --startup-project .
```

### Run the Application

```sh
dotnet run --project BookManagementAPI.Api
```

## API Endpoints

### **Book Operations**

| Method   | Endpoint                                      | Description                |
| -------- | --------------------------------------------- | -------------------------- |
| `GET`    | `/api/books/popular-books?page=1&pageSize=10` | Get popular books          |
| `GET`    | `/api/books/{id}`                             | Get a book by ID           |
| `POST`   | `/api/books/add-single`                       | Add a single book          |
| `POST`   | `/api/books/add-bulk`                         | Add multiple books         |
| `PUT`    | `/api/books/{id}`                             | Update a book              |
| `DELETE` | `/api/books/{id}`                             | Soft delete a book         |
| `POST`   | `/api/books/delete-bulk`                      | Soft delete multiple books |
| `POST`   | `/api/books/{id}/restore`                     | Restore a deleted book     |

## API Documentation (Swagger)

Once the application is running, open **Swagger UI** at:

```
http://localhost:5042/swagger/index.html
```

## Future Improvements

-  Implement Authentication & Authorization (JWT)
-  Add Unit Tests & Integration Tests
-  Implement User Roles (Admin, User)


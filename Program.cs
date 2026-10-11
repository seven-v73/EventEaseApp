using System.ComponentModel.DataAnnotations;
using UserManagementApi.Middleware;
using UserManagementApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 1. Register Custom Logging Middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// In-memory data store for demonstration
var users = new List<User>
{
    new User { Id = 1, Name = "Alice Smith", Email = "alice@example.com", Role = "Admin" },
    new User { Id = 2, Name = "Bob Jones", Email = "bob@example.com", Role = "User" }
};

// --- CRUD ENDPOINTS ---

// GET: Retrieve all users
app.MapGet("/api/users", () => Results.Ok(users))
   .WithName("GetAllUsers");

// GET: Retrieve a user by ID
app.MapGet("/api/users/{id:int}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null 
        ? Results.Ok(user) 
        : Results.NotFound(new { message = $"User with ID {id} was not found." });
})
.WithName("GetUserById");

// POST: Create a new user (with payload validation & duplicate check)
app.MapPost("/api/users", (CreateUserDto dto) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(dto);

    if (!Validator.TryValidateObject(dto, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults.Select(v => v.ErrorMessage));
    }

    if (users.Any(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
    {
        return Results.Conflict(new { message = "A user with this email address already exists." });
    }

    var newUser = new User
    {
        Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1,
        Name = dto.Name,
        Email = dto.Email,
        Role = dto.Role
    };

    users.Add(newUser);
    return Results.CreatedAtRoute("GetUserById", new { id = newUser.Id }, newUser);
})
.WithName("CreateUser");

// PUT: Update an existing user
app.MapPut("/api/users/{id:int}", (int id, CreateUserDto dto) =>
{
    var existingUser = users.FirstOrDefault(u => u.Id == id);
    if (existingUser is null)
    {
        return Results.NotFound(new { message = $"User with ID {id} was not found." });
    }

    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(dto);

    if (!Validator.TryValidateObject(dto, validationContext, validationResults, true))
    {
        return Results.BadRequest(validationResults.Select(v => v.ErrorMessage));
    }

    if (users.Any(u => u.Id != id && u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
    {
        return Results.Conflict(new { message = "Email is already registered to another user." });
    }

    existingUser.Name = dto.Name;
    existingUser.Email = dto.Email;
    existingUser.Role = dto.Role;

    return Results.Ok(existingUser);
})
.WithName("UpdateUser");

// DELETE: Remove a user by ID
app.MapDelete("/api/users/{id:int}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null)
    {
        return Results.NotFound(new { message = $"User with ID {id} was not found." });
    }

    users.Remove(user);
    return Results.NoContent();
})
.WithName("DeleteUser");

app.Run();
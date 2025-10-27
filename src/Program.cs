using BugStore.Data;
using BugStore.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

var app = builder.Build();

app.MapPost("/customers", async (Customer customer, AppDbContext context) => 
{
    customer.Id = Guid.NewGuid();

    if (string.IsNullOrEmpty(customer.Name) ||
        string.IsNullOrEmpty(customer.Email) ||
        string.IsNullOrEmpty(customer.Phone))
        return Results.BadRequest();

    context.Customers.Add(customer);
    await context.SaveChangesAsync();

    return Results.Created();
});

app.MapGet("/customers", (AppDbContext context) =>
{
    var customers = context.Customers.ToList();

    return Results.Ok(customers);
});

app.Run();

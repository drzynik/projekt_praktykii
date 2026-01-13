using praktyki_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja DbContext z SQLite w g³ównym folderze
builder.Services.AddDbContext<dbcontext>(options =>
    options.UseSqlite("Data Source=database.db"));

// Dodaj CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:5176",
                "http://localhost:5174",
                "http://localhost:7216",
                "http://localhost:5175"//tutaj zamiast tych dwóch trzeba daæ linki do tych podstron na których bêdziecie korzystaæ z endpointów
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();
app.Run();
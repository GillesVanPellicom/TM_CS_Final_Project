using Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS to allow MAUI app (desktop/android) during development
builder.Services.AddCors(options => {
  options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod());
});

// Database - MySQL via EF Core (Pomelo)
var connString = builder.Configuration.GetConnectionString("MySql")
                 ?? "server=localhost;port=3306;database=notesdb;user=notes;password=notespwd";

builder.Services.AddDbContext<NotesDbContext>(options =>
  options.UseMySql(connString, ServerVersion.AutoDetect(connString)));

var app = builder.Build();

// Migrate/Create DB and seed a few notes on startup (dev/demo)
using (var scope = app.Services.CreateScope()) {
  var db = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
  db.Database.EnsureCreated();
  if (!db.Notes.Any()) {
    db.Notes.AddRange(new Note {
      Title = "First Note",
      Content = "This is the first note.",
      Date = DateTime.UtcNow
    }, new Note {
      Title = "Second Note",
      Content = "This is the second note.",
      Date = DateTime.UtcNow
    });
    db.SaveChanges();
  }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Requests;

namespace TodoApi.Routes;

public static class TodoRoutes
{
    public static void MapTodoRoutes(this WebApplication app)
    {
        app.MapGet("/todos", async (TodoDbContext db) =>
            await db.Todos.ToListAsync());

        app.MapPost("/todos", async (CreateTodoRequest request, TodoDbContext db) =>
        {
            var todo = new Todo { Title = request.Title, IsCompleted = false };
            db.Todos.Add(todo);
            await db.SaveChangesAsync();
            return Results.Created($"/todos/{todo.Id}", todo);
        });

        app.MapPatch("/todos/{id}/complete", async (int id, TodoDbContext db) =>
        {
            var todo = await db.Todos.FindAsync(id);
            if (todo is null) return Results.NotFound();
            todo.IsCompleted = !todo.IsCompleted;
            await db.SaveChangesAsync();
            return Results.Ok(todo);
        });

        app.MapDelete("/todos/{id}", async (int id, TodoDbContext db) =>
        {
            var todo = await db.Todos.FindAsync(id);
            if (todo is null) return Results.NotFound();
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
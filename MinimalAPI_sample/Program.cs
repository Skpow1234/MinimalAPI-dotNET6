using Microsoft.EntityFrameworkCore;
using MinimalAPI_sample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoDB>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();


app.MapPost("/todoitems", async (Todo todo, TodoDB db) =>

{
   
    db.Todos.Add(todo);
    await db.SaveChangesAsync();


    return Results.Created($"/todoitems/{todo.Id}", todo);

});

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDB db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.isComplete = inputTodo.isComplete;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDB db) =>
{
    if(await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    return Results.NotFound();
});

app.MapGet("/todoitems", async(TodoDB db) => await db.Todos.ToListAsync());
app.MapGet("/todoitems/complete", async (TodoDB db) => await db.Todos.Where(t => t.isComplete).ToListAsync());
app.MapGet("/todoitems/{id}", async (int id, TodoDB db) => await db.Todos.FindAsync(id) is Todo todo ? Results.Ok(todo):Results.NotFound());


app.Run();

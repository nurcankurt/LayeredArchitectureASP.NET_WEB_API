using Microsoft.EntityFrameworkCore;
using MyProject.BusinessLogicLayer;
using MyProject.DataAccessLayer;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PersonDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("sqlConnection"),
    x => x.MigrationsAssembly("MyProject.WebApi")));
builder.Services.AddScoped<IPersonService, PersonService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/people", async (IPersonService service) =>
{
    var people = await service.GetAllPeople();
    return Results.Ok(people);
});

app.MapGet("/api/people/{id:int}", async (int id, IPersonService service) =>
{
    var person = await service.GetPersonById(id);

    if (person is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(person);
});

app.MapPost("/api/people", async (MyProject.DataAccessLayer.Models.Person person, IPersonService service) =>
{
    if (person is null)
    {
        return Results.BadRequest();
    }

    await service.CreatePerson(person);
    return Results.Created($"/api/people/{person.Id}", person);
});

app.MapPut("/api/people/{id:int}", async (int id, MyProject.DataAccessLayer.Models.Person person, IPersonService service) =>
{
    var entity = await service.GetPersonById(id);

    if (entity is null)
    {
        return Results.NotFound();
    }

    await service.UpdatePerson(id, person);

    return Results.Ok(service.GetAllPeople());
});

app.MapDelete("/api/people/{id:int}", async (int id, IPersonService service) =>
{
    var entity = await service.GetPersonById(id);

    if (entity is null)
    {
        return Results.NotFound(new
        {
            statusCode = StatusCodes.Status404NotFound,
            message = $"Person with id:{id} could not be found."
        });
    }

    await service.DeletePerson(id);
    return Results.Ok(service.GetAllPeople());
});

app.Run();

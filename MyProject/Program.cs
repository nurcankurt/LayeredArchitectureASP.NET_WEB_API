using Microsoft.EntityFrameworkCore;
using MyProject.BusinessLogicLayer;
using MyProject.DataAccessLayer;
using MyProject.WebApi;

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

PersonEndpoints.Map(app);

app.Run();

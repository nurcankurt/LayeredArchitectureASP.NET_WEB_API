using MyProject.BusinessLogicLayer;

namespace MyProject.WebApi
{
    public class PersonEndpoints
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/", () => "Hello World!");

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
        }
    }
}

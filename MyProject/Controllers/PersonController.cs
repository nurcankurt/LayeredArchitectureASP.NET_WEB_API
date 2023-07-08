using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProject.BusinessLogicLayer;

namespace MyProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _service;
        public PersonController(IPersonService service) 
        { 
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllPeople()
        {
            try
            {
                var people = _service.GetAllPeople();
                return Ok(people);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetPerson([FromRoute(Name = "id")] int id)
        {
            try
            {
                var person = _service.GetPersonById(id);


                if (person is null)
                    return NotFound(); //404

                return Ok(person);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult CreatePerson([FromBody] DataAccessLayer.Models.Person person)
        {
            try
            {
                if (person is null)
                    return BadRequest(); // 400 

                _service.CreatePerson(person);
                return StatusCode(201, person);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdatePerson([FromRoute(Name = "id")] int id,
            [FromBody] DataAccessLayer.Models.Person person)
        {
            try
            {
                
                var entity = _service.GetPersonById(id);

                if (entity is null)
                    return NotFound(); // 404

                // check id
                if (id != person.Id)
                    return BadRequest(); // 400

                return Ok(person);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletePerson([FromRoute(Name = "id")] int id)
        {
            try
            {
                var entity = _service.GetPersonById(id);

                if (entity is null)
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = $"Person with id:{id} could not found."
                    });  // 404

                _service.DeletePerson(entity);

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}

using API.Models.Dto;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ProgramsService service;

        public ProgramsController(IMapper mapper, ProgramsService service)
        {
            this.mapper = mapper;
            this.service = service;
        }

        // GET api/<ProgramsController>/5
        [HttpGet()]
        public async Task<ActionResult> Get()
        {
            var programs = await service.GetAllProgramsAsync();
            return Ok(mapper.Map<List<ProgramDto>>(programs));
        }

        [HttpGet("{id}", Name = "GetProgram")]
        public async Task<ActionResult> GetAsync(string id)
        {
            var program = await service.GetProgramByIdAsync(id);
            return program == null ? NotFound() : Ok(mapper.Map<ProgramDto>(program));
        }

        // POST api/<ProgramsController>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] CreateProgramDto programDto)
        {
            var program = mapper.Map<Models.Program>(programDto);
            if (await service.AddProgramsAsync(program))
            {
                return CreatedAtRoute(
                    "GetProgram",
                    new
                    {
                        id = program.ProgramId
                    },
                    program);
            }

            ModelState.AddModelError("", "Error occurred: Unable to create Program");
            return StatusCode(500, ModelState);

        }

        // PUT api/<ProgramsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(string id, [FromBody] ProgramDto programDto)
        {
            var program = await service.GetProgramByIdWithNoTrackingAsync(id);
            if (program == null)
            {
                return NotFound();
            }

            var programToUpdate = mapper.Map<Models.Program>(programDto);
            if (await service.UpdateProgramAsync(programToUpdate))
            {
                return NoContent();
            }

            ModelState.AddModelError("", "Error occurred: Unable to update Program");
            return StatusCode(500, ModelState);
        }

        // DELETE api/<ProgramsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var program = await service.GetProgramByIdAsync(id);
            if (program == null)
            {
                return NotFound();
            }

            await service.DeleteProgramAsync(program);
            return NoContent();
        }
    }
}

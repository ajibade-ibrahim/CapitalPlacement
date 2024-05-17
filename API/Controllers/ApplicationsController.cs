using API.Models;
using API.Models.Dto;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ProgramsService service;

        public ApplicationsController(IMapper mapper, ProgramsService service)
        {
            this.mapper = mapper;
            this.service = service;
        }

        // GET api/<ApplicationsController>/5
        [HttpGet("{programId}/{applicationId}", Name = "GetApplication")]
        public async Task<ActionResult> Get(string programId, string applicationId)
        {
            var application = await service.GetApplicationAsync(programId, applicationId);
            return application == null ? NotFound() : Ok(application);
        }

        // POST api/<ApplicationsController>
        [HttpPost()]
        public async Task<ActionResult> PostAsync([FromBody] CreateApplicationDto applicationDto)
        {
            var program = await service.GetProgramByIdAsync(applicationDto.ProgramId);
            if (program == null)
            {
                return NotFound();
            }

            var application = mapper.Map<Application>(applicationDto);
            if(await service.AddApplication(application))
            {
                return CreatedAtRoute(
                    "GetApplication",
                    new
                    {
                        applicationId = application.ApplicationId,
                        programId = application.ProgramId
                    },
                    application);
            }

            ModelState.AddModelError("", "Error occurred: Unable to create application");
            return StatusCode(500, ModelState);
            
        }
    }
}

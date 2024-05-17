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
    public class QuestionsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ProgramsService service;

        public QuestionsController(IMapper mapper, ProgramsService service)
        {
            this.mapper = mapper;
            this.service = service;
        }

        // GET: api/<QuestionsController>
        [HttpGet]
        public async Task<ActionResult> Get(string programId)
        {
            var questions = await service.GetQuestionsForProgramAsync(programId);
            return Ok(mapper.Map<List<Question>>(questions));
        }

        // GET api/<QuestionsController>/5
        [HttpGet("{programId}/{questionId}", Name ="GetQuestion")]
        public async Task<ActionResult> Get(string programId, string questionId)
        {
            var question = await service.GetQuestionByIdAsync(programId, questionId);
            return question == null ? NotFound() : Ok(question);
        }

        // POST api/<QuestionsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateQuestionDto questionDto)
        {
            var program = await service.GetProgramByIdAsync(questionDto.ProgramId);
            if (program == null)
            {
                ModelState.AddModelError("", "Program not found");
                StatusCode(404, ModelState);
            }

            var question = mapper.Map<Question>(questionDto);

            if (await service.AddQuestionAsync(question))
            {
                return CreatedAtRoute(
                    "GetQuestion",
                    new
                    {
                        programId = question.ProgramId,
                        questionId = question.QuestionId
                    },
                    question);
            }

            ModelState.AddModelError("", "Error occurred: Unable to create Program");
            return StatusCode(500, ModelState);
        }

        // PUT api/<QuestionsController>/5
        [HttpPut("{programId}/{questionId}")]
        public async Task<ActionResult> PutAsync(string programId, string questionId, [FromBody] Question questionDto)
        {
            var question = await service.GetQuestionByIdWithNoTrackingAsync(programId, questionId);
            if (question == null)
            {
                return NotFound();
            }

            var questionToUpdate = mapper.Map<Question>(questionDto);
            if (await service.UpdateQuestionAsync(questionToUpdate))
            {
                return NoContent();
            }

            ModelState.AddModelError("", "Error occurred: Unable to update question");
            return StatusCode(500, ModelState);
        }

        // DELETE api/<QuestionsController>/5
        [HttpDelete("{programId}/{questionId}")]
        public async Task<ActionResult> DeleteAsync(string programId, string questionId)
        {
            var question = await service.GetQuestionByIdAsync(programId, questionId);
            if (question == null)
            {
                return NotFound();
            }

            await service.DeleteQuestionAsync(question);
            return NoContent();
        }
    }
}

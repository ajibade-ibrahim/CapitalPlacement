namespace API.Models.Dto
{
    public class CreateApplicationDto
    {
        public string ProgramId { get; set; }
        public Person Person { get; set; }
        public IList<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }
}

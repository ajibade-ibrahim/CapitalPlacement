namespace API.Models.Dto
{
    public class QuestionDto
    {
        public string QuestionId { get; set; }
        public string ProgramId { get; set; }
        public string Content { get; set; }
        public QuestionType QuestionType { get; set; }
        public IList<string>? Options { get; set; }
        public int? MaxOptions { get; set; }
    }
}

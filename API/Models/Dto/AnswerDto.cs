namespace API.Models.Dto
{
    public class AnswerDto
    {
        public string QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public QuestionType QuestionType { get; set; }
        public string? Text { get; set; }
        public int? Number { get; set; }
        public DateTime? Date { get; set; }
        public IList<string>? SelectedOptions { get; set; }
    }
}

namespace API.Models
{
    public class Question
    {
        public Question(QuestionType questionType, string content, string programId)
        {
            Content = content;
            QuestionType = questionType;
            QuestionId = Guid.NewGuid().ToString();
            ProgramId = programId;
        }
        public string QuestionId { get; set; }
        public string ProgramId { get; set; }
        public string Content { get; set; }
        public QuestionType QuestionType { get; set; }
        public IList<string>? Options { get; set; }
        public int? MaxOptions { get; set; }
    }

    public enum QuestionType
    {
        Paragraph, YesNo, Dropdown, MultipleChoice, Date, Number
    }
}

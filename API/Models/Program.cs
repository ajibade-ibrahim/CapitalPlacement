namespace API.Models
{
    public class Program
    {
        public string ProgramId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<Question> Questions { get; set; } = new List<Question>();
    }
}

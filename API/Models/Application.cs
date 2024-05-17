namespace API.Models
{
   public class Application
    {
        public string ApplicationId { get; set; }
        public string ProgramId { get; set; }
        public Person Person { get; set; }
        public DateTime ApplicationDate { get; set; }
        public IList<Answer> Answers { get; set; } = new List<Answer>();
    }
}

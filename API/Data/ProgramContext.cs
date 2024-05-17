using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ProgramContext : DbContext
    {
        public ProgramContext(DbContextOptions<ProgramContext> options) : base(options)
        { }

        #region DbSets

        public DbSet<Question> Questions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Models.Program> Programs { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Program>()
              .ToContainer(nameof(Models.Program))
              .HasPartitionKey(program => program.ProgramId)
              .HasKey(program => program.ProgramId);

            modelBuilder.Entity<Question>()
              .ToContainer(nameof(Models.Program))
              .HasPartitionKey(question => question.ProgramId)
              .HasKey(question => question.QuestionId);

            modelBuilder.Entity<Question>()
            .Property(question => question.QuestionType).HasConversion<string>();

            modelBuilder.Entity<Application>()
              .ToContainer(nameof(Models.Program))
              .HasPartitionKey(application => application.ProgramId)
              .HasKey(application => application.ApplicationId);
            modelBuilder.Entity<Application>().OwnsMany(application => application.Answers, answers =>
            {
                answers.WithOwner().HasForeignKey(answer => answer.ApplicationId);
                answers.HasKey(answer => answer.AnswerId);
                answers.Property(answer => answer.QuestionType).HasConversion<string>();
            });
        }
    }

}

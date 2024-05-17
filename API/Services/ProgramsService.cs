using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class ProgramsService
    {
        private readonly IDbContextFactory<ProgramContext> contextFactory;
        private readonly ProgramContext context;

        public ProgramsService(ProgramContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> AddProgramsAsync(Models.Program program)
        {
            program.ProgramId = Guid.NewGuid().ToString();
            context.Add(program);
            int result = await context.SaveChangesAsync();
            return result >= 0;
        }

        public async Task<Models.Program?> GetProgramByIdAsync(string programId)
        {
            return await context.Programs.FirstOrDefaultAsync(p => p.ProgramId == programId);
        }

        public async Task<Models.Program?> GetProgramByIdWithNoTrackingAsync(string programId)
        {
            return await context.Programs.AsNoTracking().FirstOrDefaultAsync(p => p.ProgramId == programId);
        }

        public async Task<List<Models.Program>> GetAllProgramsAsync()
        {
            return await context.Programs.ToListAsync();
        }

        public async Task<bool> DeleteProgramAsync(Models.Program program)
        {
            context.Programs.Remove(program);
            var result = await context.SaveChangesAsync();
            return result >= 0;
        }

        public async Task<bool> DeleteQuestionAsync(Question question)
        {
            context.Questions.Remove(question);
            var result = await context.SaveChangesAsync();
            return result >= 0;
        }

        public async Task AddProgramWithQuestionsAsync()
        {
            await RecreateDatabase();
            string programId = Guid.NewGuid().ToString();
            var program = new Models.Program()
            {
                ProgramId = programId,
                Description = "This is a program",
                Title = "Program With Questions",
                Questions = new List<Question>
                {
                    new Question(QuestionType.Paragraph, "Who are you?", programId),
                    new Question(QuestionType.Number, "How old are you?", programId),
                    new Question(QuestionType.Date, "When were you born?", programId),
                    new Question(QuestionType.Dropdown, "What continent are you in?", programId),
                    new Question(QuestionType.MultipleChoice, "What languages do you speak?", programId)
                    {
                        Options = new List<string> {"English", "French", "German", "Spanish"},
                        MaxOptions = 3
                    },
                    new Question(QuestionType.YesNo, "Do you play football?", programId)
                }
            };

            context.Add(program);
            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateProgramAsync(Models.Program program)
        {
            return await UpdateAsync(program);
        }

        public async Task<bool> UpdateQuestionAsync(Question question)
        {
            return await UpdateAsync(question);
        }

        private async Task<bool> UpdateAsync<T>(T entity)
        {
            context.Update(entity);
            var result = await context.SaveChangesAsync();
            return result >= 0;
        }

        public async Task EditProgramsAsync()
        {
            var program = new Models.Program()
            {
                ProgramId = "b248070b-6d9d-4624-a981-8d8aed804ad4",
                Description = "This is a program update",
                Title = "Program 2",
            };

            context.Update(program);
            await context.SaveChangesAsync();
        }

        public async Task EditQuestionAsync()
        {
            var program = await context.Programs
               .SingleOrDefaultAsync(p => p.ProgramId == "704e6593-c564-4076-9d47-a1193e95020f");

            var question = await context.Questions
                .WithPartitionKey("704e6593-c564-4076-9d47-a1193e95020f")
                .SingleOrDefaultAsync(q => q.QuestionId == "61655ea6-ed08-459d-bed2-b210f881b445");

            if (question != null)
            {
                question.Content = "Tell us about yourself";
                context.Update(question);
                await context.SaveChangesAsync();
            }

            var question2 = new Question(QuestionType.Date, "What date were you born", "704e6593-c564-4076-9d47-a1193e95020f");
            question2.QuestionId = "a2f02928-a43c-40fb-b99b-620da72b68b3";
            context.Update(question2);
            await context.SaveChangesAsync();
        }

        public async Task SubmitApplicationAsync()
        {
            var person = new Person
            {
                CurrentResidence = "Brussels",
                DateOfBirth = new DateTime(1980, 1, 1),
                Email = "juninho@ol.com",
                FirstName = "Juninho",
                LastName = "Pernambucano",
                Gender = "Male",
                IDNumber = "123456",
                Nationality = "Brazil",
                Phone = "1234567890",
            };

            var application = new Application
            {
                ApplicationId = Guid.NewGuid().ToString(),
                ProgramId = "704e6593-c564-4076-9d47-a1193e95020f",
                Answers = new List<Answer>
                {
                    new Answer
                    {
                        AnswerId = Guid.NewGuid().ToString(),
                        QuestionId = "61655ea6-ed08-459d-bed2-b210f881b445",
                        QuestionContent = "Tell us about yourself",
                        QuestionType = QuestionType.Paragraph,
                        Text = "I am a football player"
                    },
                    new Answer
                    {
                        AnswerId = Guid.NewGuid().ToString(),
                        QuestionId = "a2f02928-a43c-40fb-b99b-620da72b68b3",
                        QuestionContent = "What date were you born",
                        QuestionType = QuestionType.Date,
                        Date = new DateTime(1980, 1, 1)
                    },
                    new Answer
                    {
                        AnswerId = Guid.NewGuid().ToString(),
                        QuestionId = "f2f02928-a43c-40fb-b99b-620da72b68b3",
                        QuestionContent = "What continent are you in?",
                        QuestionType = QuestionType.Dropdown,
                        Text = "Europe"
                    },
                    new Answer
                    {
                        AnswerId = Guid.NewGuid().ToString(),
                        QuestionId = "b2f02928-a43c-40fb-b99b-620da72b68b3",
                        QuestionContent = "What languages do you speak?",
                        QuestionType = QuestionType.MultipleChoice,
                        SelectedOptions = new List<string> {"English", "French"}
                    },
                    new Answer
                    {
                        AnswerId = Guid.NewGuid().ToString(),
                        QuestionId = "c2f02928-a43c-40fb-b99b-620da72b68b3",
                        QuestionContent = "Do you play football?",
                        QuestionType = QuestionType.YesNo,
                        Text = "Yes"
                    }
                }
            };

            application.Person = person;
            application.ApplicationDate = DateTime.UtcNow;
            // var json = JsonSerializer.Serialize(application);
            context.Add(application);
            await context.SaveChangesAsync();
        }

        public async Task<bool> AddApplication(Application application)
        {
            application.ApplicationId = Guid.NewGuid().ToString();
            application.ApplicationDate = DateTime.UtcNow;
            foreach (var answer in application.Answers)
            {
                answer.AnswerId = Guid.NewGuid().ToString();
                answer.ApplicationId = application.ApplicationId;
            }

            context.Add(application);
            var result = await context.SaveChangesAsync();
            return result >= 0;
        }

        public async Task GetApplication()
        {
            var application = await context.Applications
               .SingleOrDefaultAsync(p => p.ApplicationId == "50d6a107-2c64-4cc5-b630-f3335a3fdcf7");
        }

        public async Task<Application?> GetApplicationAsync(string programId, string applicationId)
        {
            return await context.Applications.WithPartitionKey(programId)
               .SingleOrDefaultAsync(p => p.ApplicationId == applicationId);
        }

        public async Task<List<Question>> GetQuestionsForProgramAsync(string programId)
        {
            return await context.Questions.WithPartitionKey(programId).ToListAsync();
        }

        public async Task<Question?> GetQuestionByIdAsync(string programId, string questionId)
        {
            return await context.Questions.WithPartitionKey(programId).SingleOrDefaultAsync(q => q.QuestionId == questionId);
        }

        public async Task<Question?> GetQuestionByIdWithNoTrackingAsync(string programId, string questionId)
        {
            return await context.Questions.WithPartitionKey(programId).AsNoTracking().SingleOrDefaultAsync(q => q.QuestionId == questionId);
        }

        private async Task RecreateDatabase()
        {
            //using var context = await contextFactory.CreateDbContextAsync();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        public async Task<bool> AddQuestionAsync(Question question)
        {
            question.QuestionId = Guid.NewGuid().ToString();
            context.Questions.Add(question);
            int result = await context.SaveChangesAsync();
            return result >= 0;
        }
    }

}

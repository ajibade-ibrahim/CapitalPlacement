using API.Models.Dto;
using AutoMapper;

namespace API
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Models.Program, CreateProgramDto>().ReverseMap();
            CreateMap<Models.Program,ProgramDto>().ReverseMap();
            CreateMap<Models.Question,CreateQuestionDto>().ReverseMap();
            CreateMap<Models.Question,QuestionDto>().ReverseMap();
            CreateMap<Models.Application,CreateApplicationDto>().ReverseMap();
            CreateMap<Models.Answer,AnswerDto>().ReverseMap();
        }
    }
}

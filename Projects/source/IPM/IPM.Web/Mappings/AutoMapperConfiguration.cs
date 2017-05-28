using AutoMapper;
using IPM.Model.Models;
using IPM.Web.Models;

namespace IPM.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<MeetingRequest, MeetingRequestViewModel>();

                cfg.CreateMap<MeetingRequestViewModel, MeetingRequest>();

                cfg.CreateMap<InterviewProcessViewModel, InterviewProcess>();

                cfg.CreateMap<InterviewProcess, InterviewProcessViewModel>();

                cfg.CreateMap<InterviewRoundViewModel, InterviewRound>();

                cfg.CreateMap<InterviewRound, InterviewRoundViewModel>();
                
                cfg.CreateMap<SkillViewModel, Skill>();

                cfg.CreateMap<Skill, SkillViewModel>();

                cfg.CreateMap<PositionViewModel, Position>();

                cfg.CreateMap<Position, PositionViewModel>();

                cfg.CreateMap<Candidate, CandidateViewModel>();

                cfg.CreateMap<CandidateViewModel, Candidate>();

                cfg.CreateMap<UserViewModel, User>();

                cfg.CreateMap<User, UserViewModel>();

                cfg.CreateMap<Interview, InterviewViewModel>();

                cfg.CreateMap<InterviewViewModel, Interview>();

                cfg.CreateMap<InterviewAnswer, InterviewAnswerViewModel>();

                cfg.CreateMap<InterviewAnswerViewModel, InterviewAnswer>();

                cfg.CreateMap<AnswerQuestion, AnswerQuestionViewModel>();

                cfg.CreateMap<AnswerQuestionViewModel, AnswerQuestion>();
            });
        }
    }
}
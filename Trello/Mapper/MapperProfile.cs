using AutoMapper;
using Trello.DTOs;
using Trello.Model;

namespace Trello.Mapper
{
    public class MapperProfile : Profile
    {

        public MapperProfile()
        {
            CreateMap<Card, CardDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Project,ProjectDetailsDto>().ReverseMap();
            CreateMap<Board, BoardDto>().ReverseMap();  
            CreateMap<Backlog, BacklogDto>().ReverseMap(); 
            CreateMap<UserStory,UserStoryDto>().ReverseMap();   
            CreateMap<Sprint,SprintDto>().ReverseMap();
            CreateMap<Card,CreateCardDto>().ReverseMap();
            CreateMap<User,UserDto>().ReverseMap();

        }
    }
}

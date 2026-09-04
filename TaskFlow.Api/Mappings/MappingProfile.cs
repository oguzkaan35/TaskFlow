using AutoMapper;
using TaskFlow.Api.DTOs.Projects;
using TaskFlow.Api.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TaskFlow.Api.Mappings;
using TaskFlow.Api.DTOs.TaskItems;
using TaskFlow.Api.DTOs.Users;

namespace TaskFlow.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProjectDto, Project>();

        CreateMap<UpdateProjectDto, Project>();

        CreateMap<Project, ProjectResponseDto>();

        CreateMap<CreateTaskItemDto, TaskItem>();

        CreateMap<UpdateTaskItemDto, TaskItem>();





        CreateMap<TaskItem, TaskItemResponseDto>()
      .ForMember(
        dest => dest.AssignedUserName,
        opt => opt.MapFrom(src => src.AssignedUser.FullName))
      .ForMember(
        dest => dest.ProjectName,
        opt => opt.MapFrom(src => src.Project.ProjectName))
      .ForMember(
        dest => dest.StatusName,
        opt => opt.MapFrom(src => src.Status.StatusName));

        CreateMap<CreateUserDto, User>()
      .ForMember(
         dest => dest.PasswordHash,
         opt => opt.MapFrom(src => src.Password)
     );




        CreateMap<UpdateUserDto, User>();

        CreateMap<User, UserResponseDto>();


    }
}

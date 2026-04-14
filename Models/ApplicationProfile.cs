using AutoMapper;
using Review.Api.Models.DTOs;

namespace Review.Api.Models;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<Project, ProjectDto>();
        CreateMap<Project, ProjectWithAssetsDto>();
        CreateMap<CreateProjectDto, Project>();
    }
}
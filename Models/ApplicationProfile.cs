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
        CreateMap<UpdateProjectDto, Project>();
        CreateMap<Comment, CommentDto>();
        CreateMap<CreateCommentDto, Comment>();
        CreateMap<Asset, AssetDto>();
        CreateMap<Asset, AssetWithCommentsDto>();
        CreateMap<CreateAssetDto, Asset>();
        CreateMap<User, UserDto>();
        CreateMap<ApiKey, ApiKeyDto>();
    }
}
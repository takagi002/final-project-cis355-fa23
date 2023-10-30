using AutoMapper;
using UserApi.Entities;
using UserApi.Models;

namespace UserApi.Mappings;

/// <summary>
/// AutoMapperProfile is used to define the mappings between different object types in the application.
/// AutoMapper is a library that simplifies the process of transforming one object type to another.
/// 
/// Each mapping configuration is defined within the constructor of this class using the CreateMap method.
/// The generic parameters of CreateMap specify the source and destination types respectively.
/// 
/// To add a new mapping configuration:
/// 1. Identify the source and destination object types.
/// 2. Add a new CreateMap<SourceType, DestinationType>() call within the constructor.
/// 3. Optionally, configure specific property mappings if the automatic mapping isn't sufficient.
///
/// Usage of this class is handled automatically by the DI container,
/// which provides an instance of IMapper to controllers or services that declare it as a dependency.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Define your mappings here
        // Example: CreateMap<SourceObjectType, DestinationObjectType>();
        CreateMap<CreateUserRequest, User>();
        CreateMap<User, CreateUserResponse>();
        CreateMap<User, AuthenticateResponse>()
                .ConstructUsing((user, context) =>
                    new AuthenticateResponse
                    {
                        Id = user.Id,
                        FirstName = user.FirstName ?? string.Empty,
                        LastName = user.LastName ?? string.Empty,
                        Username = user.Username,
                        Token = context.Items["Token"].ToString() ?? string.Empty
                    });
        // Add more mappings as needed
    }
}

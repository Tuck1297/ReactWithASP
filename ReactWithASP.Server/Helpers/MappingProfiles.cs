using AutoMapper;
using ReactWithASP.Server.Models;
using ReactWithASP.Server.Models.InputModels;
using ReactWithASP.Server.Models.OutputModels;

namespace ReactWithASP.Server.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>().ReverseMap();

            CreateMap<ConnectionStrings, ConnectionStringsDto>();
            CreateMap<ConnectionStringsDto, ConnectionStrings>().ReverseMap();

            CreateMap<RegisterInputModel, User>()
             .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User")); // You may set a default role or handle it as needed

            CreateMap<User, RegisterInputModel>()
             .ForMember(dest => dest.ConfirmedPasswordHash, opt => opt.Ignore()) // Ignore ConfirmedPasswordHash during mapping
             .ReverseMap();

            CreateMap<ConnectionStringInputModel, ConnectionStrings>();
            CreateMap<ConnectionStrings, ConnectionStringInputModel>().ReverseMap();

            CreateMap<UserOutputModel, User>();
            CreateMap<User, UserOutputModel>().ReverseMap();

            CreateMap<UpdateUserModel, User>();
            CreateMap<User, UpdateUserModel>().ReverseMap();
        }
    }

    internal class ConnectionStringsDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }

    internal class UserDto
    {
        public Guid Id { get; set; }
        public string DbName { get; set; }
        public string DbType { get; set; }
        public string DbEncryptedConnectionString { get; set; }

    }
}

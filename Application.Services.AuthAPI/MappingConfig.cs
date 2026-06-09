using Application.Services.AuthAPI.Models;
using Application.Services.AuthAPI.Models.Dtos;
using AutoMapper;

namespace Application.Services.AuthAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ApplicationUser, UserDto>();
                config.CreateMap<UserDto, ApplicationUser>();
            });

            return mapperConfig;
        }
    }
}

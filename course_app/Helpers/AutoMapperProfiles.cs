using AutoMapper;
using course_app.Dtos;
using course_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_app.Helpers
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain)))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
                
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain)))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            CreateMap<UserForUpdateDto, User>();

            CreateMap<Photo, PhotoForDetailedDto>();

            CreateMap<PhotoForCreationDto, Photo>();

            CreateMap<Photo, PhotoForReturnDto>();

            CreateMap(typeof(PagedList<>), typeof(PagedListDto<>));
        }

    }
}

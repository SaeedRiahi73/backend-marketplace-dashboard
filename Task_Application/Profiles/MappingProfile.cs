using AutoMapper;
using Task_Application.Dtos.Product;
using Task_Application.Dtos.User;
using Task_Domain.Entities;

namespace Task_Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap()
                .ForMember(u => u.PasswordHash, d => d.MapFrom(i => i.Password)); ;
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
        }
    }
}

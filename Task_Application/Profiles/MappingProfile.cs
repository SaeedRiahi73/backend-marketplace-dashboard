using AutoMapper;
using Task_Application.Dtos.Product;
using Task_Application.Dtos.User;
using Task_Application.Models.Product;
using Task_Application.Models.User;
using Task_Domain.Entities;

namespace Task_Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(
                    destination => destination.RoleId,
                    options => options.MapFrom(source => (int)source.Role))
                .ForMember(
                    destination => destination.Role,
                    options => options.MapFrom(source => source.Role.ToString()))
                .ForMember(
                    destination => destination.CanChangeStatus,
                    options => options.Ignore());

            CreateMap<UserDetailsReadModel, GetUserByIdDto>()
                .ForMember(
                    destination => destination.RoleId,
                    options => options.MapFrom(source => (int)source.Role))
                .ForMember(
                    destination => destination.Role,
                    options => options.MapFrom(source => source.Role.ToString()))
                .ForMember(
                    destination => destination.CanChangeStatus,
                    options => options.Ignore());

            CreateMap<User, CurrentUserDetailsDto>()
                .ForMember(
                    destination => destination.RoleId,
                    options => options.MapFrom(source => (int)source.Role))
                .ForMember(
                    destination => destination.Role,
                    options => options.MapFrom(source => source.Role.ToString()));

            CreateMap<UserListReadModel, UserDto>()
                .ForMember(
                    destination => destination.RoleId,
                    options => options.MapFrom(source => (int)source.Role))
                .ForMember(
                    destination => destination.Role,
                    options => options.MapFrom(source => source.Role.ToString()))
                .ForMember(
                    destination => destination.CanChangeStatus,
                    options => options.Ignore());

            CreateMap<User, CreateUserDto>().ReverseMap()
                .ForMember(u => u.PasswordHash, d => d.MapFrom(i => i.Password));

            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Product, UpdateProductDto>().ReverseMap();

            CreateMap<ProductListReadModel, ProductListItemDto>()
                .ForMember(
                    destination => destination.StatusId,
                    options => options.MapFrom(source => (int)source.Status))
                .ForMember(
                    destination => destination.Status,
                    options => options.MapFrom(source => source.Status.ToString()));

            CreateMap<ProductSummaryReadModel, ProductSummaryDto>();
        }
    }
}

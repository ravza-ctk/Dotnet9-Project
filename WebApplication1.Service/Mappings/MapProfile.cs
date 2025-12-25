using AutoMapper;
using WebApplication1.Core.DTOs;
using WebApplication1.Core.Entities;

namespace WebApplication1.Service.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserRegisterDto>().ReverseMap();
        
        CreateMap<Collection, CollectionDto>().ReverseMap(); 
        
        CreateMap<Merchandise, MerchandiseDto>()
            .ForMember(dest => dest.CollectionName, opt => opt.MapFrom(src => src.Collection.Name))
            .ReverseMap();
        CreateMap<Merchandise, MerchandiseCreateDto>().ReverseMap();
        CreateMap<Merchandise, MerchandiseUpdateDto>().ReverseMap();
        
        CreateMap<Purchase, PurchaseDto>()
             .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
             .ReverseMap();
             
        CreateMap<PurchaseItem, PurchaseItemDto>()
             .ForMember(dest => dest.MerchandiseName, opt => opt.MapFrom(src => src.Merchandise.Name))
             .ReverseMap();
        
        CreateMap<Purchase, CreatePurchaseDto>().ReverseMap();
        CreateMap<PurchaseItem, CreatePurchaseItemDto>().ReverseMap();
    }
}



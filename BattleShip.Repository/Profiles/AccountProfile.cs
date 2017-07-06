using AutoMapper;
using BattleShip.Database.Entities;
using BattleShip.Repository.RepositoryHelpers;
using BattleShip.Repository.ViewModels;

namespace BattleShip.Repository.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AddAccountViewModel, Account>()
                .ForMember(dest => dest.Password,
                    src => src.MapFrom(e => PasswordHelper.GetSha512CngPasswordHash(e.Password)))
                .ForMember(dest => dest.FirstName,
                    src => src.MapFrom(e => e.FirstName ?? string.Empty))
                .ForMember(dest => dest.LastName,
                    src => src.MapFrom(e => e.LastName ?? string.Empty))
                .ForMember(dest => dest.AllowPrivateChat,
                    src => src.MapFrom(e => true))
                .ForMember(dest => dest.AllowNewBattle,
                    src => src.MapFrom(e => true));

            CreateMap<EditAccountViewModel, Account>()
                .ForMember(m => m.Photo, opt => opt.Ignore());
        }
    }
}
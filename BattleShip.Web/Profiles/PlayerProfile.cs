using AutoMapper;
using BattleShip.Database.Entities;
using BattleShip.Web.ViewModels;

namespace BattleShip.Web.Profiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            CreateMap<Account, ExtendedEditAccountViewModel>();

            CreateMap<ExtendedEditAccountViewModel, Account>()
                .ForMember(m => m.Photo, opt => opt.Ignore());
        }
    }
}
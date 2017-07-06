using AutoMapper;
using BattleShip.Database.Entities;
using BattleShip.Repository.ViewModels;
using BattleShip.Web.ViewModels;
using Castle.Facilities.TypedFactory;

namespace BattleShip.Web.Profiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            CreateMap<Account, ExtendedEditAccountViewModel>();

            CreateMap<ExtendedEditAccountViewModel, Account>()
                .ForMember(m=>m.Photo,opt=>opt.Ignore());
        }
    }
}
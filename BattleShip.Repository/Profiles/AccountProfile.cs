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
                .AfterMap((src, dest) => dest.Password = PasswordHelper.GetSha512CngPasswordHash(src.Password))
                .AfterMap((src, dest) => dest.FirstName = src.FirstName ?? string.Empty)
                .AfterMap((src, dest) => dest.LastName = src.LastName ?? string.Empty)
                .AfterMap((src, dest) => dest.AllowPrivateChat = true)
                .AfterMap((src, dest) => dest.AllowNewBattle = true);

            CreateMap<EditAccountViewModel, Account>()
                .ForMember(m => m.Photo, opt => opt.Ignore());
        }
    }
}

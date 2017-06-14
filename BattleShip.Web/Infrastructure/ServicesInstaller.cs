using BattleShip.Database;
using BattleShip.Repository.InDatabase;
using BattleShip.Repository.Interfaces;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace BattleShip.Web.Infrastructure
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
                .Register(Component
                    .For<BattleShipContext>());

            container
                .Register(Component
                    .For<IAccountRepository>()
                    .ImplementedBy<AccountInDatabaseRepository>());

            container
                .Register(Component
                    .For<IAccountRoleRepository>()
                    .ImplementedBy<AccountRoleInDatabaseRepository>());
        }
    }
}
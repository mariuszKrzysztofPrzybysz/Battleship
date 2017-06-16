using Castle.Windsor;
using Castle.Windsor.Installer;

namespace BattleShip.Web.Infrastructure
{
    public static class ContainerManager
    {
        private static IWindsorContainer _container = null;
        public static IWindsorContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new WindsorContainer().Install(FromAssembly.This());
                }
                return _container;
            }

        }
    }
}
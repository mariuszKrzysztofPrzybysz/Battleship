using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BattleShip.Web.Infrastructure;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace BattleShip.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer _container;

        private static void BoostratpContainer()
        {
            _container = new WindsorContainer().Install(FromAssembly.This());

            var controllerFactory = new WindsorControllerFactory(_container.Kernel);

            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BoostratpContainer();
        }

        protected void Application_End()
        {
            _container.Dispose();
        }
    }
}
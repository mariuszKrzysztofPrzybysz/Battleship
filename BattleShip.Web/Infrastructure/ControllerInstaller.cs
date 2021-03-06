﻿using System.Web.Http.Controllers;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace BattleShip.Web.Infrastructure
{
    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
                .Register(Classes
                    .FromThisAssembly()
                    .BasedOn<IController>()
                    .LifestyleTransient());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using VictoryLinkTask.DI;
using VictoryLinkTask.Repository.Implementation.Common;
using VictoryLinkTask.Repository.Interfaces.Common;
using VictoryLinkTask.Service.Implementation;
using VictoryLinkTask.Service.Interfaces;

namespace VictoryLinkTask
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
            container.RegisterType<IPromotionService, PromotionService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityDependencyResolver(container);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

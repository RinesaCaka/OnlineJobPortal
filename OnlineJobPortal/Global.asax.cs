using OnlineJobPortal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.Mvc5;
using Unity;
using OnlineJobPortal.Services;

namespace OnlineJobPortal
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new UnityContainer();
            container.RegisterType<IEmployerRepository, EmployerRepository>();
            container.RegisterType<IPublicRepository, PublicRepository>();
            container.RegisterType<IJobSeekerRepository, JobSeekerRepository>();

            container.RegisterType<IEmployerService, EmployerService>();
            container.RegisterType<IPublicService, PublicService>();
            container.RegisterType<IJobSeekerService, JobSeekerService>();


            // Register the Unity container as the MVC dependency resolver
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
    }
}

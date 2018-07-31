using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;

namespace WebApplication1.Util
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {            
            var builder = new ContainerBuilder();            
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<CreateTwoDimensionalArray>().As<ICreateRandomArray>();
            builder.RegisterType<CreateCSV>().As<ICreateFile>();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
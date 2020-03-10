﻿using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using ProductData.ApplicationServices.Interface;
using ProductData.ApplicationServices.Services;
using ProductData.Data.SqlDataContext;

namespace ProductData.Web
{
    public class ContainerConfig
    {
        public static void RegisterContainer(HttpConfiguration httpConfiguration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ProductDataContext>()
                .InstancePerRequest();

            builder.RegisterType<ProductDataServices>()
                .As<IProductDataServices>()
                .InstancePerRequest();

            // MVC
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //API
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build();

            // MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //API
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
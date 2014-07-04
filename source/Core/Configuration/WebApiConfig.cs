﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license
 */
using Owin;
using System;
using System.IO;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;

namespace Thinktecture.IdentityManager
{
    public class WebApiConfig
    {
        public static void Configure(HttpConfiguration apiConfig, IDependencyResolver dependencyResolver)
        {
            if (apiConfig == null) throw new ArgumentNullException("apiConfig");
            if (apiConfig == null) throw new ArgumentNullException("dependencyResolver");

            apiConfig.MapHttpAttributeRoutes();
            apiConfig.DependencyResolver = dependencyResolver;

            apiConfig.SuppressDefaultHostAuthentication();
            apiConfig.Filters.Add(new HostAuthenticationAttribute("Bearer"));
            //apiConfig.Filters.Add(new AuthorizeAttribute(){Roles=config.AdminRoleName});

            apiConfig.Formatters.Remove(apiConfig.Formatters.XmlFormatter);
            apiConfig.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            //apiConfig.Services.Add(typeof(IExceptionLogger), new UserAdminExceptionLogger());
        }

        public static void Configure(HttpConfiguration httpConfig, IdentityManagerConfiguration idmConfig)
        {
            if (httpConfig == null) throw new ArgumentNullException("httpConfig");
            if (idmConfig == null) throw new ArgumentNullException("idmConfig");

            var resolver = AutofacConfig.Configure(idmConfig);
            Configure(httpConfig, resolver);
        }

        public class UserAdminExceptionLogger : ExceptionLogger
        {
            public override void Log(ExceptionLoggerContext context)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
                path = Path.Combine(path, "UserAdminException.txt");
                Directory.CreateDirectory(path);
                var msg = DateTime.Now.ToString() + Environment.NewLine + context.Exception.ToString() + Environment.NewLine + Environment.NewLine;
                File.AppendAllText(path, msg);
            }
        }
    }
}

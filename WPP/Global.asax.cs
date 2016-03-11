
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Web.Common;
using WPP.Service.BaseServiceClasses;
using System.IO;

namespace WPP
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public static ISessionFactory SessionFactory;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
          //  configureNHibernate();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

        }


        protected void Application_BeginRequest()
        {
            CultureInfo cInf = new CultureInfo("en-US", false);

            cInf.DateTimeFormat.DateSeparator = "/";
            cInf.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            cInf.DateTimeFormat.LongDatePattern = "dd/MM/yyyy hh:mm:ss tt";

            System.Threading.Thread.CurrentThread.CurrentCulture = cInf;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cInf;
        }


    }
}
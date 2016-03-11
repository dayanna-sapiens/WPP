[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WPP.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WPP.App_Start.NinjectWebCommon), "Stop")]

namespace WPP.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using WPP.Service.BaseServiceClasses;
    using Ninject.Extensions.Conventions;
    using Ninject.Extensions.Interception;
    using WPP.Security;
    using System.Web.Security;
    using WPP.Service.ModuloContratos;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Inicia la aplicación
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        


        /// <summary>
        /// Detiene la aplicación
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Se crea el kernel que administrará la aplicación
        /// </summary>
        /// <returns>kernel creado</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Se realizar la inyección de dependencias 
        /// </summary>
        /// <param name="kernel">kernel</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind(x => x.FromAssembliesMatching("*").SelectAllClasses().Excluding<UnitOfWork>().BindDefaultInterface());
            kernel.Bind<RoleProvider>().To<WPPRolesProvider>();

        }
    }
}

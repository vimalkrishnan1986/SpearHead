using Ninject;
using Ninject.Extensions.Wcf.SelfHost;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceProcess;
using SpearHead.Contracts;
using SpearHead.Contracts.Implementation;
using SpearHead.Host.NinjectModules;
using Ninject.Modules;
using Ninject.Extensions.Wcf;
using SpearHead.ServiceContracts;
using Ninject.Web.Common.SelfHost;

namespace SpearHead.Host
{
    partial class HostingService : ServiceBase
    {
        #region Privare Valriables
        private readonly object _lockObject = new object();
        private static List<Assembly> assemblies = new List<Assembly>();
        #endregion

        #region Internal Variables
        internal List<Type> ServiceTypes = new List<Type>();
        #endregion


        private static IKernel CreateKernel()
        {
            IKernel kernel = new StandardKernel(new INinjectModule[] {
                    new ServiceModule(),new DataModule(),new BusinessModule()
                     });
            kernel.Load(assemblies.ToArray());
            return kernel;
        }

        public HostingService()
        {
            InitializeComponent();
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.TypeResolve += new ResolveEventHandler(CurrentDomain_TypeResolve);

        }

        internal void InternalStart()
        {
            this.OnStart(null);
        }
        protected override void OnStart(string[] args)
        {
            try
            {
                this.StartService();
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        protected override void OnStop()
        {
        }

        #region Private Methods
        private void StartService()
        {
            lock (_lockObject)
            {
                var uplodService = NinjectWcfConfiguration.Create<ExcelUploadService, NinjectServiceSelfHostFactory>();
                var selfHost = new NinjectSelfHostBootstrapper(
                CreateKernel,
                uplodService);
                Console.Write($"Starting the service {uplodService.BaseAddresses.SingleOrDefault()}");
                selfHost.Start();
            }

        }
        private void Log(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        #endregion

        #region Static methods


        static Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            string[] files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*.dll");
            foreach (string fileName in files)
            {
                Assembly loadedAssembly;
                try
                {
                    loadedAssembly = Assembly.LoadFrom(fileName);
                    assemblies.Add(loadedAssembly);
                    var loadedTypes = loadedAssembly.GetExportedTypes();
                    foreach (var type in loadedTypes)
                    {
                        if (type.FullName == args.Name)
                        {
                            return loadedAssembly;
                        }
                    }
                }
                catch
                {

                }
            }
            return null;
        }
        #endregion
    }
}

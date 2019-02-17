using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceProcess;

namespace SpearHead.Host
{
    partial class HostingService : ServiceBase
    {
        #region Privare Valriables
        private readonly object _lockObject = new object();
        private List<ServiceHost> Services = new List<ServiceHost>();
        #endregion

        #region Internal Variables
        internal List<Type> ServiceTypes = new List<Type>();
        #endregion

        public HostingService()
        {
            InitializeComponent();
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.TypeResolve += new ResolveEventHandler(CurrentDomain_TypeResolve);
            ServiceModelSectionGroup mssg = ServiceModelSectionGroup.GetSectionGroup(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
            foreach (ServiceElement service in mssg.Services.Services)
            {
                var serviceType = Type.GetType(service.Name, true, true);
                ServiceTypes.Add(serviceType);
            }
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
            Services.ForEach(thishost =>
            {
                if (IsListening(thishost))
                {

                    thishost.Close();
                }

            });
        }

        #region Private Methods
        private bool IsListening(ICommunicationObject serviceHost)
        {

            if (serviceHost == null)
            {
                return false;
            }
            return serviceHost.State == CommunicationState.Opened || serviceHost.State == CommunicationState.Opening;
        }
        private void StartService()
        {
            lock (_lockObject)
            {
                ServiceTypes.ForEach(type =>
                {
                    var service = new ServiceHost(type);
                    Services.Add(service);
                    Console.WriteLine(string.Format("Opening the Service {0}\n", service.BaseAddresses.SingleOrDefault()));
                    service.Open();
                }

                 );
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

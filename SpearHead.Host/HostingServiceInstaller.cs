using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.ServiceProcess.Design;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpearHead.Host
{
    [RunInstaller(true)]
    public partial class HostingServiceInstaller : Installer
    {
        public HostingServiceInstaller()
        {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
        }


        protected override void OnBeforeInstall(System.Collections.IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);
        }

        protected override void OnBeforeUninstall(System.Collections.IDictionary savedState)
        {
            base.OnBeforeUninstall(savedState);
        }

        protected override void OnBeforeRollback(System.Collections.IDictionary savedState)
        {
            base.OnBeforeRollback(savedState);
        }
    }
}

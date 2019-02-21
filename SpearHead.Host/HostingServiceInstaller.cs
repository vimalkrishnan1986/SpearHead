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
            GetServiceAccount(ref ServiceProcessInstaller);
            RetrieveServiceName();
            base.Install(stateSaver);
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            RetrieveServiceName();
            base.Uninstall(savedState);
        }

        // Prompt the user for service installation account values.
        public static bool GetServiceAccount(ref ServiceProcessInstaller svcInst)
        {
            bool accountSet = false;
            ServiceInstallerDialog svcDialog = new ServiceInstallerDialog();

            // Query the user for the service account type.
            do
            {
                svcDialog.TopMost = true;
                svcDialog.StartPosition = FormStartPosition.CenterScreen;
                svcDialog.ShowDialog();

                if (svcDialog.Result == ServiceInstallerDialogResult.OK)
                {
                    // Do a very simple validation on the user
                    // input.  Check to see whether the user name
                    // or password is blank.

                    if ((svcDialog.Username.Length > 0))
                    {
                        // Use the account and password.
                        accountSet = true;
                        svcInst.Account = ServiceAccount.User;
                        svcInst.Username = svcDialog.Username;

                        if (!string.IsNullOrEmpty(svcDialog.Password))
                        {
                            svcInst.Password = svcDialog.Password;
                        }
                    }
                }
                else if (svcDialog.Result == ServiceInstallerDialogResult.UseSystem)
                {
                    svcInst.Account = ServiceAccount.LocalSystem;
                    svcInst.Username = null;
                    svcInst.Password = null;
                    accountSet = true;
                }

                if (!accountSet)
                {
                    DialogResult result;
                    result = MessageBox.Show(
                        "Invalid user name or password for service installation." +
                        "  Press Cancel to leave the service account unchanged.",
                        "Change Service Account",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Hand);

                    if (result == DialogResult.Cancel)
                    {
                        // Break out of loop.
                        break;
                    }
                }
            } while (!accountSet);

            return accountSet;
        }

        private void RetrieveServiceName()
        {
            this.ServiceInstaller.ServiceName = Context.Parameters["ServiceName"];
            this.ServiceInstaller.DisplayName = Context.Parameters["ServiceDisplayName"];
            this.ServiceInstaller.Description = Context.Parameters["ServiceDescription"];
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

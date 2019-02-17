using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SpearHead.Host
{
    class Program
    {
        const string consoleCommand = "-console";

        static void Main(string[] args)
        {
            if (args.Contains(consoleCommand))
            {
                HostingService host = new HostingService();
                Console.Write("Starting windows service using command line \n");
                host.InternalStart();
                Console.ReadKey();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
        {
                new HostingService()
        };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}

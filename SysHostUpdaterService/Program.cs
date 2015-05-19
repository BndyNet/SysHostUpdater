using System.ServiceProcess;

namespace SysHostsUpdaterService
{
    static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[] 
            { 
                new UpdateSysHosts() 
            };
			ServiceBase.Run(ServicesToRun);
		}
	}
}

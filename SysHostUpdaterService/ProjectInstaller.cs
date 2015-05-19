using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace SysHostsUpdaterService
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : System.Configuration.Install.Installer
	{
		public const string ServiceName = "UpdateSysHostsSvc";
		public ProjectInstaller()
		{
			InitializeComponent();

			this.serviceInstaller1.ServiceName = ServiceName;
		}

		public override void Commit(IDictionary savedState)
		{
			base.Commit(savedState);

			using (ServiceController sc = new ServiceController(this.serviceInstaller1.ServiceName))
			{
				if (sc != null
					&& sc.Status != ServiceControllerStatus.Running
					&& sc.Status != ServiceControllerStatus.StartPending)
				{
					sc.Start();
				}
			}
		}
	}
}

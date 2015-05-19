using System;
using System.Collections;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Windows;
using System.Windows.Threading;

namespace SysHostsUpdaterGui
{
    public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			CheckSvc();
		}

		private void btnRun_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("SysHostsUpdaterCmd.exe");
		}

		private void btnInsSvc_Click(object sender, RoutedEventArgs e)
		{
			InstallService();
		}

		private void btnUninsSvc_Click(object sender, RoutedEventArgs e)
		{
			UninstallService();
		}
		private void CheckSvc()
		{
			this.btnInsSvc.IsEnabled = true;
			this.btnUninsSvc.IsEnabled = false;

			var svcName = SysHostsUpdaterService.ProjectInstaller.ServiceName;
			foreach (ServiceController sc in ServiceController.GetServices())
			{
				if (sc.ServiceName == svcName)
				{
					this.btnInsSvc.IsEnabled = false;
					this.btnUninsSvc.IsEnabled = true;
				}
			}
		}

		private void InstallService()
		{
			this.IsEnabled = false;
			this.Dispatcher.Invoke(new Action(() =>
			{
				IDictionary savedState = new Hashtable();
				using (AssemblyInstaller ins = new AssemblyInstaller())
				{
					ins.UseNewContext = true;
					ins.Path = "SysHostsUpdaterService.exe";
					ins.Install(savedState);
					ins.Commit(savedState);
					ins.Dispose();
				}

				this.IsEnabled = true;
				CheckSvc();
			}), DispatcherPriority.Background);
		}
		private void UninstallService()
		{
			this.IsEnabled = false;
			this.Dispatcher.Invoke(new Action(() =>
			{
				IDictionary savedState = new Hashtable();
				using (AssemblyInstaller ins = new AssemblyInstaller())
				{
					ins.UseNewContext = true;
					ins.Path = "SysHostsUpdaterService.exe";
					ins.Uninstall(savedState);
					ins.Dispose();
				}
				this.IsEnabled = true;
				CheckSvc();
			}), DispatcherPriority.Background);
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

namespace SysHostsUpdaterService
{
	using SysHostsUpdater;
	public partial class UpdateSysHosts : ServiceBase
	{
		private const string LogName = "CommerNet";
		private Timer _timer;
		private Updater _updater;
		public UpdateSysHosts()
		{
			InitializeComponent();

			this.AutoLog = true;

			_updater = new Updater();

			_updater.LogHandler = WriteLog;
		}

		protected override void OnStart(string[] args)
		{
			if (EventLog.SourceExists(this.ServiceName))
			{
				var logName = EventLog.LogNameFromSourceName(this.ServiceName, ".");
				if (logName != LogName)
				{
					EventLog.DeleteEventSource(this.ServiceName);
				}
			}
			if (!EventLog.SourceExists(this.ServiceName))
			{
				EventLog.CreateEventSource(this.ServiceName, LogName);
			}

			_updater.ReplaceSysHosts();

			_timer = new Timer(1 * 60 * 60 * 1000);	// 1 hour
			_timer.Elapsed += (sender, e) =>
			{
				_updater.ReplaceSysHosts();
			};
			_timer.Start();
		}

		protected override void OnStop()
		{
			_timer.Stop();
			_timer.Dispose();
		}


		private void WriteLog(string message, EventLogEntryType type = EventLogEntryType.Information)
		{
			var eventLog = new EventLog(LogName);
			eventLog.Source = this.ServiceName;
			eventLog.WriteEntry(message, type);
		}
	}
}

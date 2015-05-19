using System;
using System.Diagnostics;

namespace SysHostsUpdaterCmd
{
    using SysHostsUpdater;
    class Program
	{
		static void Main(string[] args)
		{
			var updater = new Updater();
			updater.LogHandler = (string _1, EventLogEntryType _2) =>
			{
				Console.WriteLine(string.Format("{0}\t{1}\t{2}",
					DateTime.Now, _2, _1));
			};
			updater.ReplaceSysHosts();

			Console.WriteLine("Press any key to exit!");
			Console.ReadKey();
		}
	}
}

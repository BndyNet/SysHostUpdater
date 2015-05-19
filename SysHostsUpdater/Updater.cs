using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SysHostsUpdater
{
	public class Updater
	{
		public void ReplaceSysHosts()
		{
			var bakFile = @"C:\Windows\System32\drivers\etc\hosts.bak";
			var hostFile = @"C:\Windows\System32\drivers\etc\hosts";

			// back up
			if (!File.Exists(bakFile))
			{
				Log("Back up host file");
				File.Copy(hostFile, bakFile);
			}

			Log("Updating host file...");

			var content = File.ReadAllText(hostFile);
			if (content.IndexOf("# Current Version") > 0)
			{
				content = content.Substring(0, content.IndexOf("# Current Version"));
			}

			content = content.Trim();

			try
			{
				content += Environment.NewLine + GetContent();

				File.WriteAllText(hostFile, content, System.Text.Encoding.UTF8);

				Log("Updated Successfully");
			}
			catch (Exception ex)
			{
				Log(ex.Message, EventLogEntryType.Warning);
			}
		}

		private string GetContent()
		{
			using (WebClient wc = new WebClient())
			{
				var url = "http://blog.yadgen.com/?page_id=585";
				var content = wc.DownloadString(url);

				url = Regex.Match(content,
					@"<a href=""(?<url>.*?)"">\d{4}\.\d{1,2}\.\d{1,2}\.txt</a>")
					.Groups["url"].Value;
				content = wc.DownloadString(url);
				content = content.Replace("\r\n", "\n")
					.Replace("\n", "\r\n");

				return content;
			}
		}

		private void Log(string message, EventLogEntryType logType = EventLogEntryType.Information)
		{
			if (LogHandler != null)
			{
				LogHandler(message, logType);
			}
		}
		public Action<string, EventLogEntryType> LogHandler;
	}
}
